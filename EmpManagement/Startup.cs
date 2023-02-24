using EmpManagement.Models;
using EmpManagement.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace EmpManagement
{
    public class Startup
    {
        private IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("EmployeeDBConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>(
                options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 3;
                    options.Password.RequireNonAlphanumeric = false;
                    options.SignIn.RequireConfirmedEmail = true;
                    options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                }).
                AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("CustomEmailConfirmation");
            services.Configure<DataProtectionTokenProviderOptions>(x => x.TokenLifespan = TimeSpan.FromMinutes(30));
            services.Configure<CustomEmailConfirmationTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromDays(3));
            services.AddMvc(options =>
            {
                var Policy = new AuthorizationPolicyBuilder().
                RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(Policy));
            });
            services.AddAuthentication().AddGoogle(x =>
            {
                x.ClientId = "566336375721-j541c0l9jj942lo9pdrtt1ohqbq326bn.apps.googleusercontent.com";
                x.ClientSecret = "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";
            }).
            AddFacebook(x =>
            {
                x.AppId = "733810531646946";
                x.AppSecret = "5318800dabcbbd36f4ee06d8fefd26a5";
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });
            services.AddAuthorization(x =>
            {
                x.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role"));
                x.AddPolicy("EditRolePolicy", policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));
                x.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("admin"));
            });
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
            services.AddSingleton<DataProtectionPurposeStrings>();
        }

        //Midddle ware
        //setup request processing pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env
        , ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithRedirects("/Error/{0}");
            }
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute("Default", "{controller=home}/{Action=Index}/{Id?}");
            });
            //app.UseMvcWithDefaultRoute();
        }
    }
}
