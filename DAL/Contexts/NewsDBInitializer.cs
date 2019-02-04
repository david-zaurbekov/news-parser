using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contexts
{
    class NewsDBInitializer : CreateDatabaseIfNotExists<NewsContext>
    {
        protected override void Seed(NewsContext context)
        {
            context.ApplicationUsers.Add(new Data.ApplicationUser
            {
                UserName = "Test",
                ApiKey = "qwerty",
                IpAddress = "::1"
            });
            context.ApplicationUsers.Add(new Data.ApplicationUser
            {
                UserName = "Test2",
                ApiKey = "qwerty",
                IpAddress = "127.0.0.1"
            });

            base.Seed(context);
        }
    }
}
