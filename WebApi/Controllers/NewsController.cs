using DAL.Repositories;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Text.RegularExpressions;
using WebApi.Models;
using log4net;

namespace WebApi.Controllers
{
    [RoutePrefix("api")]
    [Authorize]
    public class NewsController : ApiController
    {
        private readonly INewsRepository repository;
        private readonly ILog logger;

        public NewsController(INewsRepository newsRepository, ILog log)
        {
            repository = newsRepository;
            logger = log;
        }

        // GET api/posts
        [HttpGet, Route("posts")]
        public IHttpActionResult GetPosts([FromUri]GetPostsViewModel model)
        {
            try
            {
                var articles = repository.Articles;
                if (model.From.HasValue)
                    articles = articles.Where(x => x.PublishDate >= model.From);
                if (model.To.HasValue)
                {
                    var zeroTime = new TimeSpan(0);
                    if (model.From.HasValue && model.From.Value.TimeOfDay == zeroTime && model.To.Value.TimeOfDay == zeroTime)
                        model.To = model.To.Value.AddSeconds(86399);
                    articles = articles.Where(x => x.PublishDate <= model.To);
                }
                return Ok(articles.ToList());
            }
            catch (Exception exc)
            {
                logger.Error(exc.Message);
                return InternalServerError(new Exception("Service temporarily unavailable"));
            }
        }

        // GET api/topten
        [HttpGet, Route("topten")]
        [Authorize]
        public IHttpActionResult GetTopWords()
        {
            try
            {
                var text = string.Join(" ", repository.Articles.Select(x => x.Content.ToLower()));
                var words = Regex.Matches(text, @"\b\w+\b").OfType<Match>()
                    .Select(x => x.Value)
                    .GroupBy(x => x)
                    .OrderByDescending(x => x.Count())
                    .Select(x => x.Key)
                    .Take(10)
                    .ToList();
                return Ok(words);
            }
            catch (Exception exc)
            {
                logger.Error(exc.Message);
                return InternalServerError(new Exception("Service temporarily unavailable"));
            }
        }

        // GET api/search
        [HttpGet, Route("search")]
        public IHttpActionResult Search(string text)
        {
            try
            {
                var articles = repository.Articles;

                if (!string.IsNullOrEmpty(text))
                    articles = articles.Where(x => x.Title.Contains(text) || x.Content.Contains(text));

                return Ok(articles.ToList());
            }
            catch (Exception exc)
            {
                logger.Error(exc.Message);
                return InternalServerError(new Exception("Service temporarily unavailable"));
            }
        }
    }
}
