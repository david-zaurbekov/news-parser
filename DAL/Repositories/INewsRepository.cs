using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface INewsRepository : IDisposable
    {
        void SaveChanges();

        Task SaveChangesAsync();

        IQueryable<Article> Articles { get; }

        void AddArticle(Article newItem);

        void AddRangeArticle(IEnumerable<Article> newItems);
    }
}
