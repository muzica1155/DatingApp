using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
            var services = scope.ServiceProvider;
            try 
            { 
                //changes After identity 
                //instad of or as well as getting the DataContext we r also going to get userManager 

                var context = services.GetRequiredService<DataContext>();

                var userManager = services.GetRequiredService<UserManager<AppUser>>();

                var roleManager = services.GetRequiredService<RoleManager<AppRole>>();//bcoz we also need to pass in the roleManager here 
                await context.Database.MigrateAsync();//MigrateAsync();// what this method doing this is for our convenience is what we r been doing so far is using dotnet ef database update 
                // await Seed.SeedUsers(context);// bcoz we haven't modified the seed class which we r 'going to look at next we r gonna to see if we can get ASPcore tocreate our database 
                // await Seed.SeedUsers(userManager);// aget this stop our application drop our database and then restart the application & this is gonna seed all of our users using the userManager now
                 await Seed.SeedUsers(userManager, roleManager);// pass in the role managet as an additional parameter here
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();//<Program>>passs in program class as its type & what we will do if we get an error will say logger 
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
