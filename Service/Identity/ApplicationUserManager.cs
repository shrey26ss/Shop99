using Data;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Service.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private readonly IDapperRepository _dapperRepository;
        private readonly ILogger<UserManager<ApplicationUser>> _logger;
        public ApplicationUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger, IDapperRepository dapperRepository) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _dapperRepository = dapperRepository;
            _logger = logger;
        }

        public async Task<ApplicationUser> FindByMobileNoAsync(string mobileNo)
        {
            var user = new ApplicationUser();
            try
            {
                string sqlQuery = @"SELECT u.Id,u.UserName,u.RefreshToken,u.[Name], r.[RoleId], ar.NormalizedName [Role], u.Email ,u.PhoneNumberConfirmed
                                FROM Users u(nolock) 
                                     LEFT join UserRoles r(nolock) on r.UserId = u.Id 
                                     LEFT join ApplicationRole ar(nolock) on ar.Id = r.RoleId 
                                WHERE u.PhoneNumber = @mobileNo";
                user = await _dapperRepository.GetAsync<ApplicationUser>(sqlQuery, new { mobileNo }, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                throw;
            }
            return user ?? new ApplicationUser();
        }

        public Task FindByMobileNoAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> GetUserByToken(LoginAPIRequest request)
        {
            var user = await _dapperRepository.GetAsync<ApplicationUser>(@"Select u.Id,u.UserName,u.RefreshToken,u.[Name], ar.[Name] as [Role], u.Email from Users u(nolock) left join UserRoles r(nolock) on r.UserId = u.Id left join ApplicationRole ar(nolock) on ar.Id = r.RoleId where u.UserId = @MerchantID and u.ConcurrencyStamp = @SecurityCode",
                new
                {
                    request.MerchantID,
                    request.SecurityCode
                }, commandType: CommandType.Text);
            return user ?? new ApplicationUser();
        }
        public override async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            var result = IdentityResult.Failed();
            try
            {
                var res = await base.ResetPasswordAsync(user, token, newPassword);
                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    string sqlQuery = @"update [dbo].[Users] set PasswordHash=@PasswordHash where Id=@Id";
                    int i = await _dapperRepository.ExecuteAsync(sqlQuery, new { user.Id, user.PasswordHash }, commandType: CommandType.Text);
                    if (i > 0)
                    {
                        result = IdentityResult.Success;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public override Task<IList<string>> GetValidTwoFactorProvidersAsync(ApplicationUser user)
        {
            return base.GetValidTwoFactorProvidersAsync(user);
        }

        public async Task<IdentityResult> SigninWithOTP(string mobileNo, string otp, bool lockoutOnFailure = false)
        {
            var result = IdentityResult.Failed();
            int i = await _dapperRepository.GetAsync<int>("proc_SignWithOTP", new { mobileNo, otp, lockoutOnFailure }, commandType: CommandType.StoredProcedure);
            if (i > 0)
            {
                result = IdentityResult.Success;
            }
            return result;
        }
         public async Task<IdentityResult> ConfirmPhoneNumber(string mobileNo, string otp, bool lockoutOnFailure = false)
        {
            var result = IdentityResult.Failed();
            int i = await _dapperRepository.GetAsync<int>("proc_ConfirmPhoneNumber", new { mobileNo, otp, lockoutOnFailure }, commandType: CommandType.StoredProcedure);
            if (i > 0)
            {
                result = IdentityResult.Success;
            }
            return result;
        }

        public async Task<Response> SaveLoginOTP(string mobileNo, string otp)
        {
            var result = new Response();
            if (string.IsNullOrEmpty(mobileNo))
            {
                result.ResponseText="Invalid MobileNo";
                return result;
            }
            else if (string.IsNullOrEmpty(otp))
            {
                result.ResponseText="Invalid OTP";
                return result;
            }
            try
            {
                var sqlQuery = @"
                             INSERT INTO RequestedOTP(MobileNo,OTP,[Action],EntryOn, IsUsed,attemptedCount) 
                             VALUES (@mobileNo,@otp,'LOGINOTP',GETDATE(),0,0)
                             Select 1 StatusCode,'OTP Send Successfully' ResponseText";
                result = await _dapperRepository.GetAsync<Response>(sqlQuery, new { mobileNo, otp }, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                result.ResponseText="Something went wrong.Please try after sometime.";
                _logger.LogError(ex, ex.Message);
            }
            return result;
        }
    }
}