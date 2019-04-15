using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;
using Model.Interfaces;
using DAL.Repositories;
using BAL.Managers;
using AutoMapper;
using BAL.Services;
using BAL.Jobs;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using BAL.Interfaces;
using BAL.Hubs;
using Microsoft.Extensions.Logging;
using NLog;
using System.IO;
using BAL.Exceptions;
using StackExchange.Redis;
using WebApp.Extensions;
using BAL.Notifications.Infrastructure;
using BAL.Notifications;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
               services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

			services.AddAuthentication().AddFacebook(facebookOptions =>
			{
				facebookOptions.AppId = "650976532012853";
				facebookOptions.AppSecret = "bafe321bce69a757c812991f4468597e";
			});

			services.AddAuthentication().AddGoogle(configureOptions =>
			{
				configureOptions.ClientId = "91528411350-j52vl6bbdp58ild09dqelr9n4ccl11vf.apps.googleusercontent.com";
				configureOptions.ClientSecret = "11May0pGIYDGLc0ZO0GNi05y";
			});
            // Add application services.
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddTransient<IEmailSender, EmailSender>();
			services.AddTransient<ITariffRepository, TariffRepository>();
			services.AddTransient<IBaseRepository<Tariff>, BaseRepository<Tariff>>();
			services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IMailingRepository, MailingRepository>();
            services.AddTransient<IEmailCampaignRepository, EmailCampaignRepository>();
            services.AddTransient<IBaseRepository<ApplicationGroup>, BaseRepository<ApplicationGroup>>();
            services.AddTransient<IAdminStatisticRepository, AdminStatisticRepository>();


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
            services.AddScoped<ISubscribeWordManager,SubscribeWordManager>();
            services.AddScoped<IGroupManager, GroupManager>();
            services.AddScoped<IOperatorManager, OperatorManager>();
            services.AddScoped<ICodeManager, CodeManager>();
            services.AddScoped<IMailingManager, MailingManager>();
			services.AddSingleton<ISmsSender, SmsSender>();
            services.AddScoped<IChartsManager, ChartsManager>();
            services.AddScoped<IAnswersCodeManager, AnswersCodeManager>();
            services.AddScoped<IRecievedMessageManager, RecievedMessageManager>();
            services.AddScoped<IEmailManager, EmailManager>();
            services.AddScoped<IEmailCampaignManager, EmailCampaignManager>();
            services.AddScoped<IEmailRecipientManager, EmailRecipientManager>();
            services.AddScoped<INotificationManager, NotificationManager>();
            services.AddScoped<IEmailMailingManager, EmailMailingManager>();
            services.AddScoped<ITestMessageManager, TestMessageManager>();
            services.AddScoped<IAdminStatisticManager, AdminStatisticManager>();
            services.AddScoped<INotificationsGenerator<EmailCampaign>, EmailCampaignNotificationGenerator>();
            services.AddScoped<INotificationsGenerator<Company>, SmsCampaignNotificationGenerator>();

            // Configure sessions

            services.AddDistributedMemoryCache();
            services.AddSession();

            // Configure hubs

            services.AddSignalR();

            // Register Jobs and JobFactory, start mailing scheduler

            services.AddTransient<JobFactory>();

            services.AddScoped<Mailing>();
            services.AddScoped<BAL.Jobs.Notification>();

        }
       
        public void Configure(IApplicationBuilder app, 
                              IHostingEnvironment env, ILoggerManager logger)
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
            app.ConfigureCustomExceptionMiddleware();
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
