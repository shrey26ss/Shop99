using Entities.Models;
using Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Identity
{
    public interface IUserService : IRepository<ApplicationUser>
    {

        Task<Response> ChangeAction(int id);
        Task<Response> AssignPackage(int userId, int packageId);
        Task<Response> Assignpackage(int TID);

        Task<Response> TwoFactorEnabled(int id);
    }
}
