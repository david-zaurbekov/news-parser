using DAL.Repositories;
using DAL.Repositories.EF;
using NewsParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleParser
{
    class Program
    {
        static void Main(string[] args)
        {
            INewsParser parser = new NurKzParser();
            var news = parser.GetNewsAsync(30).Result;
            INewsRepository newsRepository = new EFNewsRepository();
            newsRepository.AddRangeArticle(news);
            newsRepository.SaveChanges();
        }
    }
}
