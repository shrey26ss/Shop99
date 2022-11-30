using Infrastructure.Interface;
using System.Threading.Tasks;

namespace Service.Identity
{
    public interface IUserService : IRepository<ApplicationUser>
    {
        Task<IResponse> ChangeAction(int id);
        Task<IResponse> AssignPackage(int userId, int packageId);
        Task<IResponse> Assignpackage(int TID);
        Task<IResponse> TwoFactorEnabled(int id);
    }
}
