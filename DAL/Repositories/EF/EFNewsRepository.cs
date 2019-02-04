using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Contexts;
using Data;

namespace DAL.Repositories.EF
{
    public class EFNewsRepository : INewsRepository
    {
        private NewsContext context = new NewsContext();

        #region IDisposable

        private bool disposed = false;

        protected void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing && context != null)
            {
                context.Dispose();
                context = null;
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        public IQueryable<Article> Articles => context.Articles.AsQueryable();

        public void AddArticle(Article newItem)
        {
            context.Articles.Add(newItem);
        }

        public void AddRangeArticle(IEnumerable<Article> newItems)
        {
            context.Articles.AddRange(newItems);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
