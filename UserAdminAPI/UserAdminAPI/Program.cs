using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAdminAPI.Infrastructure;

namespace UserAdminAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
       
            using (var scope = host.Services.CreateScope())
            {              
                var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
         
                if (!db.Model.GetEntityTypes().Any())   // if doesn't exist any table in database
                    db.Database.Migrate(); // apply the migrations
            }

            host.Run(); // start handling requests
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
