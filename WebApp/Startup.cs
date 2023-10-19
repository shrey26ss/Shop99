using Entities.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Services.Identity;
using System;
using System.Linq;
using System.Text;
using WebApp.Middleware;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.RegisterService(Configuration);
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddCookie("Cookies", options =>
            {
                options.LoginPath = "/Account/login";
                options.AccessDeniedPath = "/Account/login";
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.SlidingExpiration = true;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
                };
            }).AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    string authorization = context.Request.Headers[HeaderNames.Authorization];
                    if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                        return "Bearer";
                    return "Cookies";
                };
            });
            services.AddControllersWithViews();
            services.AddMvc();
            services.AddHsts(option =>
            {
                option = new Microsoft.AspNetCore.HttpsPolicy.HstsOptions
                {
                    IncludeSubDomains = true,
                    Preload = true,
                    MaxAge = TimeSpan.FromDays(30),
                };
            });
            services.AddIdentity<ApplicationUser, ApplicationRole>();
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = false;
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.Configure<JWTConfig>(Configuration.GetSection("JWT"));
            services.AddDataProtection().SetApplicationName($"{WebHostEnvironment.EnvironmentName}")
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo($@"{WebHostEnvironment.ContentRootPath}\keys"));
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            });

            // Add response compression middleware
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true; // Enable compression for HTTPS requests
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "text/html" }); // Compress HTML files
            });

            // Add Brotli compression
            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest; // Adjust compression level as needed
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Infrastructure.Interface.ILog logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/Status404");
                app.UseHsts();
            }
            app.UseResponseCompression();
            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
            app.UseSession();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseMiddleware<UserAgentMiddleware>();
            app.UseMiddleware<WebsiteinfoMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

}