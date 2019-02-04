using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using Data;
using RestSharp;

namespace NewsParser
{
    /// <summary>
    /// Парсер новостей с сайта nur.kz
    /// </summary>
    public class NurKzParser : INewsParser
    {
        private const string baseUrl = "https://www.nur.kz/daynews/?page={page}";
        private const int defaultNewsCount = 30;
        private const int maxPages = 5;
        private readonly int maxThreads = 4;

        public NurKzParser()
        {
            int processorsCount = Environment.ProcessorCount;
            maxThreads = processorsCount < maxThreads ? processorsCount : maxThreads;
        }

        /// <summary>
        /// Извлекает последние n новостей с сайта
        /// </summary>
        /// <param name="count">Количество извлекаемых новостей</param>
        /// <returns>Возвращает перечисление новостей</returns>
        public async Task<IEnumerable<Article>> GetNewsAsync(int count = defaultNewsCount)
        {
            if (count <= 0)
                count = defaultNewsCount;

            var news = new List<Article>();

            var httpClient = new RestClient(baseUrl);
            for (int i = 1; i <= maxPages; i++)
            {
                try
                {
                    var request = new RestRequest(Method.GET);
                    request.AddUrlSegment("page", i);

                    IRestResponse response = await httpClient.ExecuteTaskAsync(request).ConfigureAwait(false);

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        continue;

                    var content = response.Content;
                    news.AddRange(ParseDocument(content, count));

                    if (news.Count >= count)
                        break;
                }
                catch (Exception)
                {
                }
            }

            if (news.Count > count)
                return news.Take(count).ToList();
            return news;
        }

        /// <summary>
        /// Выполняет парсинг страницы сос писком новостей
        /// </summary>
        /// <param name="content">Html документ</param>
        /// <returns>Возвращает новости на данной странице</returns>
        private IEnumerable<Article> ParseDocument(string content, int maxCount)
        {
            var newsUrls = new List<string>();
            var parser = new HtmlParser();
            using (var document = parser.ParseDocument(content))
            {
                var articleUrls = document.QuerySelectorAll("article > a");
                if (articleUrls != null)
                {
                    foreach (var a in articleUrls)
                    {
                        var href = a.GetAttribute("href");
                        if (!string.IsNullOrEmpty(href) && !newsUrls.Contains(href))
                            newsUrls.Add(href);
                        if (newsUrls.Count == maxCount)
                            break;
                    }
                }
            }

            var news = new ConcurrentBag<Article>();

            if (newsUrls.Any())
            {
                var options = new ParallelOptions() { MaxDegreeOfParallelism = newsUrls.Count < maxThreads ? newsUrls.Count : maxThreads };
                Parallel.ForEach(newsUrls, options, url => ParseNewsPage(url, news));
            }

            return news.ToList();
        }

        /// <summary>
        /// Выполняет извлечение информации со страницы новости
        /// </summary>
        /// <param name="url">URL адрес новости</param>
        /// <param name="list">Список куда будет помещен результат</param>
        private void ParseNewsPage(string url, ConcurrentBag<Article> list)
        {
            var client = new RestClient(url);
            try
            {
                var request = new RestRequest(Method.GET);
                var response = client.Execute(request);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    return;

                var content = response.Content;
                var htmlParser = new HtmlParser();
                using (var document = htmlParser.ParseDocument(content))
                {
                    Article item = new Article() { Url = url };
                    var article = document.QuerySelector(".layout-article-page__content");
                    if (article != null)
                    {
                        var dateTime = article.QuerySelector("time")?.GetAttribute("datetime");
                        if (DateTime.TryParse(dateTime, out DateTime date))
                            item.PublishDate = date;

                        item.Title = article.QuerySelector(".article-headline")?.TextContent;
                        item.Content = string.Join(Environment.NewLine, article.QuerySelectorAll(".formatted-body p")?.Select(x => x.TextContent));
                    }
                    if (!string.IsNullOrEmpty(item.Title) && !string.IsNullOrEmpty(item.Content))
                        list.Add(item);
                }
            }
            catch (Exception) { }
        }
    }
}
