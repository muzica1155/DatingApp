using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        // public static void Main(string[] args)
        public static async Task Main(string[] args)//we r the main methods inside the program class we r outside of our middleware // we setting u a global exceptions handler we dont have access to it in this method write a try & cathc method
        {
            // CreateHostBuilder(args).Build().Run(); //remove run  & assign this method to a variable 
            var host = CreateHostBuilder(args).Build();
            //we need to do get our service our data context service so that we can pass it to our seed method
            using var scope = host.Services.CreateScope();// do it crateng scope for the service that we r going to create in htis part 
            var _services = scope.ServiceProvider;
            try 
            { 
                var context = _services.GetRequiredService<DataContext>();
                await context.Database.MigrateAsync();//MigrateAsync();// what this method doing this is for our convenience is what we r been doing so far is using dotnet ef database update 
                await Seed.SeedUsers(context);
            }
            catch (Exception ex)
            {
                var logger = _services.GetRequiredService<ILogger<Program>>();//<Program>>passs in program class as its type & what we will do if we get an error will say logger 
                logger.LogError(ex, "An error occurred during migration");
            
            }
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
