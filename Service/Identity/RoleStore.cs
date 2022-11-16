using Data;
using Microsoft.AspNetCore.Identity;
using Service.Identity;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Identity
{
    public class RoleStore : IRoleStore<ApplicationRole>, IQueryableRoleStore<ApplicationRole>
    {
        //private readonly ApplicationDbContext _context;
        private readonly IDapperRepository _dapperRepository;
        public RoleStore(ApplicationDbContext context, IDapperRepository dapperRepository)
        {
            //_context = context;
            _dapperRepository = dapperRepository;
        }

        // public IQueryable<ApplicationRole> Roles => _context.Roles();
        public IQueryable<ApplicationRole> Roles => AllRoles();

        public IQueryable<ApplicationRole> AllRoles()
        {
            var result = _dapperRepository.GetAsQueryable<ApplicationRole>("select * from ApplicationRole", null, commandType: CommandType.Text);
            return result;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            int i = await _dapperRepository.ExecuteAsync("AddRole", role, commandType: CommandType.StoredProcedure);
            return i > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }

        public Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var role = await _dapperRepository.GetAsync<ApplicationRole>("select * from ApplicationRole where Id='" + roleId + "'", null, commandType: CommandType.Text);
            return role;
        }

        public async Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var role = await _dapperRepository.GetAsync<ApplicationRole>("select * from ApplicationRole where NormalizedName='" + normalizedRoleName + "'", null, commandType: CommandType.Text);
            return role;
        }

        public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.FromResult(0);
        }

        public Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
