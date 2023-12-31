﻿using Data;
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
using Service.Product;
using Service.Variant;
using Service.Vendor;
using Service.TopBanners;
using Service.TopLowerBanners;
using Service.Homepage;
using Service.CatAttrMappings;
using Service.CartWishList;
using Service.Countries;
using Service.State;
using Service.City;
using Service.Address;
using Service.CatBrandMapping;
using Service.OrderDetails;
using Service.Report;
using Service.Dashboard;
using Service.Notify;
using Service.Offers;
using AppUtility.AppCode;
using AppUtility.Helper;
using Service.Developer;
using Service.Websiteinfo;

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
            services.AddScoped<IUserService, Service.Identity.UserService>();
            services.AddScoped<IAPILogger, APILogger>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICartWishListService, CartWishListService>();
            services.AddScoped<IBrands, BrandService>();
            services.AddScoped<IAttributes, AttributeService>();
            services.AddScoped<IProducts, ProductService>();
            services.AddScoped<IPlaceOrder, PlaceOrderService>();
            services.AddScoped<IPGCallback, PGCallbackService>();
            services.AddScoped<IVariant, VariantService>();
            services.AddScoped<IVendor, VendorService>();
            services.AddScoped<ITopBanner, TopBannerService>();
            services.AddScoped<ITopLowerBanner, TopLowerBannerService>();
            services.AddScoped<IDeveloper, DeveloperService>();
            services.AddScoped<ILog, LogNLog>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddScoped<Data.Migrations.Database>();
            services.AddScoped<IHomepageService, HomepageService>();
            services.AddScoped<IFiltersService, FiltersService>();
            services.AddScoped<ICategoryAttributeMapping, CategoryAttributeMappingService>();
            services.AddScoped<ICategoryBrandMapping, CategoryBrandMappingService>();
            services.AddScoped<IProductHomeService, ProductHomeService>();
            services.AddScoped<ICountry, CountryService>();
            services.AddScoped<IStates, StateService>();
            services.AddScoped<ICity, CityService>();
            services.AddScoped<IUserAddress, UserAddressService>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IReport, ReportService>();          
            services.AddScoped<INotifyService, NotifyService>();          
            services.AddScoped<IOrderDetailsService, OrderDetailsService>();
            services.AddScoped<IRequestInfo, RequestInfoServcie>();
            services.AddScoped<IDashboard, DashboardService>();
            services.AddScoped<IOffersService, OffersService>();
            services.AddScoped<IWebsiteinfo, WebsiteinfoService>();
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
            PaymentServiceSetting paymentService = new PaymentServiceSetting();
            configuration.GetSection("PaymentServiceSetting").Bind(paymentService);
            services.AddSingleton(paymentService);
            services.AddSingleton<OSInfoAPI>();
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1.1",
                    Title = "API Documentation(v1.1)"
                });
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "ApiDoc.xml");
                option.IncludeXmlComments(filePath);
                //option.OperationFilter<AddRequiredHeaderParameter>();
                option.UseAllOfToExtendReferenceSchemas();
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Standard authorization header using the bearer scheme(\"Bearer {token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                //option.OperationFilter<SecurityRequirementsOperationFilter>();
                option.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
            //services.AddSwaggerGen(option =>
            //{
            //    option.SchemaFilter<SwaggerIgnoreFilter>();
            //    option.SwaggerDoc("v1", new OpenApiInfo
            //    {
            //        Version = "v1.1"
            //    });
            //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "ApiDoc.xml");
            //    option.IncludeXmlComments(filePath);
            //    //option.OperationFilter<AddRequiredHeaderParameter>();
            //    option.UseAllOfToExtendReferenceSchemas();
            //});
        }
    }
}
