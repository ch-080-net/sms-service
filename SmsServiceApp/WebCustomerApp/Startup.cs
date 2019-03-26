using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Model.Interfaces;
using DAL.Repositories;
using BAL.Managers;
using AutoMapper;
using BAL.Services;
using BAL.Jobs;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using WebApp;
using Microsoft.Extensions.Localization;
using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using System.Threading;
using BAL.Interfaces;
using BAL.Hubs;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
			services.AddTransient<ITariffRepository, TariffRepository>();
			services.AddTransient<IBaseRepository<Tariff>, BaseRepository<Tariff>>();
			services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IMailingRepository, MailingRepository>();
            services.AddTransient<IBaseRepository<ApplicationGroup>, BaseRepository<ApplicationGroup>>();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en-US"),
                        new CultureInfo("uk-UA")
                    };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                };
            });


            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings  
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings  
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings  
                options.User.RequireUniqueEmail = true;
            });


            //Seting the Account Login page  
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings  
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Account/NewLogin"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login  
                options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout  
                options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied  
                options.SlidingExpiration = true;
            });
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
               .AddDataAnnotationsLocalization()   
               .AddViewLocalization();
                



            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<IRecipientManager, RecipientManager>();
            services.AddScoped<IContactManager, ContactManager>();
            services.AddScoped<ITariffManager, TariffManager>();
            services.AddScoped<IPhoneManager, PhoneManager>();
            services.AddScoped<IStopWordManager, StopWordManager>();
            services.AddScoped<IGroupManager, GroupManager>();
            services.AddScoped<IOperatorManager, OperatorManager>();
            services.AddScoped<ICodeManager, CodeManager>();
            services.AddScoped<IMailingManager, MailingManager>();
			services.AddSingleton<ISmsSender, SmsSender>();
            services.AddScoped<IChartsManager, ChartsManager>();
            services.AddScoped<IAnswersCodeManager, AnswersCodeManager>();
            services.AddScoped<IRecievedMessageManager, RecievedMessageManager>();

            // Configure sessions

            services.AddDistributedMemoryCache();
            services.AddSession();

            // Configure hubs

            services.AddSignalR();

            // Register Jobs and JobFactory

            services.AddTransient<JobFactory>();

            services.AddScoped<Mailing>();
            services.AddScoped<BAL.Jobs.Notification>();

        }
       
        public void Configure(IApplicationBuilder app, 
                              IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
           
 
            app.UseRequestLocalization(); 
            app.UseStaticFiles();
            app.UseAuthentication();


            // Configure sessions

            app.UseSession();
            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificationHub>("/notificationHub");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{id?}");    
            });

        }
    }
}
