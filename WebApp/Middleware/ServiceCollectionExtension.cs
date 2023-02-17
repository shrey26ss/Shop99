using Data;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Identity;
using Service.Identity;
using Infrastructure.Repos;
using Infrastructure.Interface;
using Service.Repos;
using WebApp.Models;
using WebApp.AppCode.Extension;
using WebApp.AppCode;
using WebApp.Servcie;
using WebApp.AppCode.Helper;
using AppUtility.Helper;
using AppUtility.AppCode;

namespace WebApp.Middleware
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterService(this IServiceCollection services, IConfiguration configuration)
        {
            string dbConnectionString = configuration.GetConnectionString("SqlConnection");
            IConnectionString ch = new ConnectionString { connectionString = dbConnectionString };
            services.AddSingleton<IViewRenderService, ViewRenderService>();            
            services.AddSingleton(ch);
            services.AddScoped<IDapperRepository, DapperRepository>();
            services.AddScoped<IHttpRequestInfo, HttpRequestInfo>();
            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<IUserStore<ApplicationUser>, UserStore>();
            services.AddScoped<IRoleStore<ApplicationRole>, RoleStore>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryAPI, CategoryAPI>();
            services.AddScoped<ICheckOutAPI, CheckOutAPI>();
            services.AddScoped<ICartWishListAPI,CartWishListAPI>();
            services.AddScoped<IProductsAPI, ProductsAPI>();
            services.AddScoped<ILog, LogNLog>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<IDDLHelper, DDLHelper>();
            services.AddSingleton<IGenericMethods, GenericMethods>();
            services.AddScoped<Data.Migrations.Database>();
            //services.AddProgressiveWebApp();
            services.AddAutoMapper(typeof(Startup));

            services.AddMemoryCache();

            //JWTConfig jwtConfig = new JWTConfig();
            //configuration.GetSection("JWT").Bind(jwtConfig);
            //services.AddSingleton(jwtConfig);

            AppSettings appSettings = new AppSettings();
            configuration.Bind(appSettings);
            services.AddSingleton(appSettings);
            services.ConfigureDictionary<ImageSize>(configuration.GetSection("ImageSize"));
            services.AddSingleton<OSInfo>();

        }
    }
}
