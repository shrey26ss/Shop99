using Data;
using Entities.Models;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Services.Identity;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.OpenApi.Models;
using System.IO;
using Service.Identity;
using FluentMigrator.Runner;
using Infrastructure.Repos;
using Infrastructure.Interface;
using Service.Repos;
using Service.Categories;
using Service.Brand;
using Service.Attribute;

namespace WebAPI.Middleware
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterService(this IServiceCollection services, IConfiguration configuration)
        {
            string dbConnectionString = configuration.GetConnectionString("SqlConnection");
            GlobalDiagnosticsContext.Set("connectionString", dbConnectionString);
            IConnectionString ch = new ConnectionString { connectionString = dbConnectionString };

            //var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailService.EmailConfiguration>();
            //services.AddSingleton(emailConfig);
            services.AddSingleton<IViewRenderService, ViewRenderService>();
            //services.AddScoped<IEmailService, EmailFactory>();
            services.AddSingleton<IConnectionString>(ch);
            services.AddScoped<IDapperRepository, DapperRepository>();
            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<IUserStore<ApplicationUser>, UserStore>();
            services.AddScoped<IRoleStore<ApplicationRole>, RoleStore>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAPILogger, APILogger>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBrands, BrandService>();
            services.AddScoped<IAttributes, AttributeService>();
            services.AddScoped<Infrastructure.Interface.ILog, LogNLog>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddScoped<Data.Migrations.Database>();
            //services.AddProgressiveWebApp();
            services.AddAutoMapper(typeof(Startup));
            services.AddHangfire(x => x.UseSqlServerStorage(dbConnectionString));
            services.AddHangfireServer();
            services.AddLogging(c => c.AddFluentMigratorConsole())
                .AddFluentMigratorCore()
                .ConfigureRunner(c => c.AddSqlServer2016()
                .WithGlobalConnectionString(configuration.GetConnectionString("SqlConnection"))
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());
            List<NotificationPermissions> notificationPermissions = new List<NotificationPermissions>();
            configuration.GetSection("NotificationPermissions").Bind(notificationPermissions);
            services.AddSingleton(notificationPermissions);
            APIConfig apiConfig = new APIConfig();
            configuration.GetSection("APIConfig").Bind(apiConfig);
            services.AddSingleton(apiConfig);
            services.AddSingleton(apiConfig);
            JWTConfig jwtConfig = new JWTConfig();
            configuration.GetSection("JWT").Bind(jwtConfig);
            services.AddSingleton(jwtConfig);
            services.AddSwaggerGen(option =>
            {
                option.SchemaFilter<SwaggerIgnoreFilter>();
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1.1"
                });
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "ApiDoc.xml");
                option.IncludeXmlComments(filePath);
                option.OperationFilter<AddRequiredHeaderParameter>();
                option.UseAllOfToExtendReferenceSchemas();
            });
        }
    }
}
