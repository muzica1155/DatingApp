using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {// write a logic so that we can get the data out of that jason file & into our database.
        // SeedUsers(Data)//name od the method
        public static async Task SeedUsers(DataContext context) /// we create s atatic method inside here so that we dont need to create a new instance of this class to ue the method that we r going to create 
         //async Task // we r running effectively void from this not given  our task parameter this will still give us asyn functionality even thought we r returning void 
        //or not returning anything will
        {
            if (await context.User.AnyAsync())//want to check to se if use this table contains any users
         return;//if we continue means no any users in our database & we want to go interrogate file to see what wehave inside there & store it in a variable table
         var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");//ReadAllTextAsync("")// location of the json file that we cerated 
         var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
         //var userData // deserialized what is inside the user data here this is gonna be a string json text & we want to deserialized that into an oject json text
        //users should be a normal list of users of type app user // we should have it out of tha json file by this points
         foreach (var user in users)
         {
              using var hmac = new HMACSHA512();

              user.UserName = user.UserName.ToLower();
              user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
              user.PasswordSalt = hmac.Key;

              context.User.Add(user);//when we r adding all we r doing dont forget is tracking adding tracking to the user through entity framewrk 
         }// go  them to our database just as we are doing in our register component
         // we still need passwords for these & then we can go & add them to our databse just as we r doing in our register componenets 
         //we still need passwords for these but we also need to use password, salt & password hash 
        await context.SaveChangesAsync();// this is our seed method 
        
        } 


    }
}