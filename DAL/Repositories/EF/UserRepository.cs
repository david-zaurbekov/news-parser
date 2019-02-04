using DAL.Contexts;
using DAL.Repositories;
using Data;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories.EF
{
    public class UserRepository<TUser> : IUserRepository<TUser> where TUser : ApplicationUser
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

        public async Task CreateAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException();
            context.ApplicationUsers.Add(user);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException();
            context.ApplicationUsers.Remove(user);
            await context.SaveChangesAsync();
        }

        public async Task<ApplicationUser> FindByIdAsync(int userId)
        {
            return await context.ApplicationUsers.FindAsync(userId);
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return await context.ApplicationUsers.FirstOrDefaultAsync(x => x.ApiKey == userName);
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException();
            context.Entry(user).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}