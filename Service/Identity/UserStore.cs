using Data;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq;
using System.Text;
using AppUtility.Helper;
using Entities.Models;

namespace Services.Identity
{
    public class UserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserEmailStore<ApplicationUser>, IUserRoleStore<ApplicationUser>, IQueryableUserStore<ApplicationUser>, IUserLockoutStore<ApplicationUser>, IUserTwoFactorStore<ApplicationUser>, IUserPhoneNumberStore<ApplicationUser>, IUserAuthenticatorKeyStore<ApplicationUser>,
        IUserTwoFactorRecoveryCodeStore<ApplicationUser>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IDapperRepository _dapperRepository;
        private readonly ILogger<UserStore> _logger;
        private IMapper _mapper;
        public UserStore(RoleManager<ApplicationRole> roleManager, IDapperRepository dapperRepository, ILogger<UserStore> logger, IMapper mapper)
        {
            _roleManager = roleManager;
            _dapperRepository = dapperRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public IQueryable<ApplicationUser> AllUsers()
        {
            var res = _dapperRepository.GetAllAsync<ApplicationUser>("select * from Users where ID<>1", null, commandType: CommandType.Text).Result;
            return res.AsQueryable();
        }
        public IQueryable<ApplicationUser> Users => AllUsers();

        public async Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            int roleid = _roleManager.FindByNameAsync(roleName).Result.Id;
            await _dapperRepository.ExecuteAsync("insert into UserRoles values('" + user.Id + "','" + roleid + "')", null, commandType: CommandType.Text);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                var param = _mapper.Map<ApplicationUserProcModel>(user);
                var res = await _dapperRepository.ExecuteAsync("AddUser", new
                {
                    param.Id,
                    param.UserId,
                    param.SecurityStamp,
                    param.PhoneNumber,
                    param.PhoneNumberConfirmed,
                    param.PasswordHash,
                    param.NormalizedUserName,
                    param.NormalizedEmail,
                    param.LockoutEnd,
                    param.LockoutEnabled,
                    param.EmailConfirmed,
                    param.Email,
                    param.ConcurrencyStamp,
                    param.AccessFailedCount,
                    param.TwoFactorEnabled,
                    param.UserName,
                    param.Role,
                    param.Name,
                    param.FOSId,
                    param.GAuthPin,
                    param.RefreshToken,
                    param.RefreshTokenExpiryTime
                }, commandType: CommandType.StoredProcedure);
                var description = Utility.O.GetErrorDescription(res);
                if (res > 0 && res < 10)
                {
                    return IdentityResult.Success;
                }
                var resp = IdentityResult.Failed(new IdentityError()
                {
                    Code = "-1",
                    Description = description
                });
                return resp;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "-1",
                    Description = ex.Message
                });
            }
        }

        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            var sqlQuery = $"select u.*, r.RoleId [Role] from Users u inner join UserRoles r on r.UserId = u.Id where u.NormalizedEmail='{normalizedEmail}'";
            var user = await _dapperRepository.GetAsync<ApplicationUser>(sqlQuery, null, commandType: CommandType.Text);
            return user ?? new ApplicationUser();
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _dapperRepository.GetAsync<ApplicationUser>("select * from Users where Id='" + userId + "'", null, commandType: CommandType.Text);
            return user ?? new ApplicationUser();
        }

        public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _dapperRepository.GetAsync<ApplicationUser>("select * from Users where NormalizedUserName='" + normalizedUserName + "'", null, commandType: CommandType.Text);
            return user;
        }

        public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.NormalizedEmail);
        }

        public async Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var normalizedUserName = await _dapperRepository.GetAsync<string>("select NormalizedUserName from Users where Email='" + user.Email + "'", null, commandType: CommandType.Text);
            return normalizedUserName;
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.PasswordHash);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                var userRoles = await _dapperRepository.GetAllAsync<string>("proc_getUserRole", new { user.Id, user.Email, mobileNo = user.PhoneNumber }, commandType: CommandType.StoredProcedure);
                return userRoles.ToList();
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user != null && user.Id != null && user.Id.ToString() != "0")
                return await Task.FromResult(user.Id.ToString());
            else
            {
                string sqlQuery = @"select Id from Users(nolock) where Email=Email or PhoneNumber=@mobileNo";
                return await _dapperRepository.GetAsync<string>(sqlQuery, new { user.Email, mobileNo = user.PhoneNumber }, commandType: CommandType.Text);
            }
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.UserName);
        }

        public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            bool result = false;
            int roleid = _roleManager.FindByNameAsync(roleName).Result.Id;
            string sqlQuery = @"IF (select Count(1) from UserRoles(nolock) where RoleId=@roleid and UserId=@Id)>0 select 1 else select 0";
            result = await _dapperRepository.GetAsync<bool>(sqlQuery, new { roleid, user.Id }, commandType: CommandType.Text);
            return result;
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);

        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);

        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);

        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                //var param = _mapper.Map<UserUpdateRequest>(user);
                var param = new UserUpdateRequest
                {
                    Id = user.Id,
                    PasswordHash = user.PasswordHash,
                    RefreshToken = user.RefreshToken,
                    RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
                    TwoFactorEnabled = user.TwoFactorEnabled
                };
                var result = await _dapperRepository.ExecuteAsync("UpdateUser", param, commandType: CommandType.StoredProcedure);
                return result > 0 ? IdentityResult.Success : IdentityResult.Failed();
            }
            catch(Exception ex)
            {
                return IdentityResult.Failed();
            }            
        }

        public async Task<int> GetAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.AccessFailedCount);
        }

        public async Task<bool> GetLockoutEnabledAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.LockoutEnabled);
        }

        //Note : if GetLockoutEnabledAsync will false GetLockoutEndDateAsync will not hit;
        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.LockoutEnd);
        }

        public async Task<int> IncrementAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return await _dapperRepository.ExecuteAsync("update [dbo].[Users] set AccessFailedCount=AccessFailedCount + 1 ,lockoutEnd=IIF(AccessFailedCount > 2?Getadte()+7,lockoutEnd) where Id=@Id", new { user.Id }, commandType: CommandType.Text);
        }

        public async Task ResetAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            await _dapperRepository.ExecuteAsync("update [dbo].[Users] set AccessFailedCount = 0  where Id = @Id", new { user.Id }, commandType: CommandType.Text);

        }

        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = enabled;
            return Task.CompletedTask;
        }

        public async Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            await _dapperRepository.ExecuteAsync("update [dbo].[Users] set lockoutEnd=@lockoutEnd where Id=@Id", new { user.Id, lockoutEnd }, commandType: CommandType.Text);
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
                return Task.FromResult(false);
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancellationToken)
        {
            string sp = "Update Users Set GAuthPin = @GAuthPin, TwoFactorEnabled = @TwoFactorEnabled where Id = @Id";
            user.TwoFactorEnabled = enabled;
            var res = _dapperRepository.ExecuteAsync(sp, new { user.TwoFactorEnabled, user.Id, user.GAuthPin }, commandType: CommandType.Text);
            return Task.CompletedTask;
        }

        public Task<string> GetPhoneNumberAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new NotImplementedException();
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new NotImplementedException();
            }
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private string GenrateRandom(int length)
        {
            string valid = "ABCDEFGHJKMNPQRSTUVWXYZ";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public Task<string> GetAuthenticatorKeyAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            string response = GenrateRandom(16);
            if (user != null && !string.IsNullOrEmpty(user.GAuthPin))
            {
                response = user.GAuthPin;
            }
            return Task.FromResult(response);
        }

        public Task SetAuthenticatorKeyAsync(ApplicationUser user, string key, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<int> CountCodesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(1);
        }

        public Task<bool> RedeemCodeAsync(ApplicationUser user, string code, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public Task ReplaceCodesAsync(ApplicationUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
