using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BAL.Jobs;
using BAL.Managers;
using BAL.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Model.Interfaces;
using WebApp.Models;
using static WebApp.Startup;
using NLog.Web;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                var host = BuildWebHost(args);
                using (var scope = host.Services.CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;
                    try
                    {
                        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                        var operatorManager = serviceProvider.GetRequiredService<IOperatorManager>();
                        var codeManager = serviceProvider.GetRequiredService<ICodeManager>();
                        var tariffManager = serviceProvider.GetRequiredService<ITariffManager>();
                        var stopWordManager = serviceProvider.GetRequiredService<IStopWordManager>();
                        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

                        IdentityDataInitializer.SeedData(userManager, roleManager, operatorManager, codeManager, tariffManager, stopWordManager, unitOfWork);

                        // Start Notification scheduler

                        NotificationScheduler.Start(scope.ServiceProvider);
                        MailingScheduler.Start(scope.ServiceProvider);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "Stopped program because of exception");
                    }
                }
                host.Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();  
    }
}
