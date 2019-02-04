using Data;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IUserRepository<TUser> : IUserStore<ApplicationUser, int>, IDisposable where TUser : ApplicationUser
    {
    }
}
