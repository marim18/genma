using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenmaWebApp.Data;
using GenmaWebApp.Models;

using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenmaWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();


            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var db = services.GetRequiredService<ApplicationDbContext>();
                    var um = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var rm = services.GetRequiredService<RoleManager<IdentityRole>>();
                    ApplicationDbInitializer.Initialize(db, um, rm); 
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
                host.Run();
            }
 
        }

        //CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });



    }
}