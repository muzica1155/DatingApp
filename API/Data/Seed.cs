using System.Collections.Generic;
using System.Security.Cryptography;
// using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {// write a logic so that we can get the data out of that jason file & into our database.
        // SeedUsers(Data)//name od the method
        // public static async Task SeedUsers(DataContext context) /// we create s atatic method inside here so that we dont need to create a new instance of this class to ue the method that we r going to create 
         //async Task // we r running effectively void from this not given  our task parameter this will still give us asyn functionality even thought we r returning void 
        //or not returning anything will

//Replace after Identity enity
        public static async Task SeedUsers(UserManager<AppUser> userManager, //we r going use to create our users now instead 
         RoleManager<AppRole> roleManager) //giving role to admin a role for a moderator 7 a role for a member & seed those roles into our database
        {
        //     if (await context.Users.AnyAsync())//want to check to se if use this table contains any users
        //  return;//if we continue means no any users in our database & we want to go interrogate file to see what wehave inside there & store it in a variable table
         
         if (await userManager.Users.AnyAsync())//Afte userManager then u add a period then we'll see all of the different methods that we have available with the userManger
         // we can use this to fin users we've got different ethods here to find by email, find by id find by login find by name & that'll be the username
         // so we've got 2 methods here that we can use to get a specific user find the ID 7 find by name Bcoz that's gonna be the username and
         //& more methods like deleteusers create users & ther's also an update user method as well & we various things with users manager Also
         //give us the access to Users table & notice this return & I query a bowl of users if the store is an equally bale user store & ours 
         //we can also use this just like our datacontent to work with the differnt users as well 
         return;

         var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");//ReadAllTextAsync("")// location of the json file that we cerated 
         var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
         //var userData // deserialized what is inside the user data here this is gonna be a string json text & we want to deserialized that into an oject json text
        //users should be a normal list of users of type app user // we should have it out of tha json file by this points

          if (users == null) return;//after we check to make sure that there r users or users in our database

          var roles = new List<AppRole>//add variable for rolls saveour rolls equals & we r gonna//new List<AppRole>// create a new list approle 
          {
              new AppRole{ Name = "Member"},
              new AppRole{ Name = "Admin"},
              new AppRole{ Name = "Moderator"},
              
          };

          foreach (var role in roles)
          {
              await roleManager.CreateAsync(role);
          }// loop through these different roles

         foreach (var user in users)
         {
              using var hmac = new HMACSHA512();

              user.UserName = user.UserName.ToLower();//we still gonna convert our username into lowercase Now there is a normalize username inside ASp.NET identity & if we go back & take another look at the database in Normalize username all r stored in capital later 

              //removed after identity bcoz we r going to use the identity functionality for this now so just remove the using for the actual
            //   user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
            //   user.PasswordSalt = hmac.Key;

            //   context.Users.Add(user);//when we r adding all we r doing dont forget is tracking adding tracking to the user through entity framewrk 
         
              await userManager.CreateAsync(user, "Pa$$w0rd");//this particular method takes the user that we r creating as an app user & there's an overload here which also takes the password that we can add here as well so we used this 
              // we specify htat we want to create a user & will hardcoded the password again & this does require a complete password inless u turn off the requirements & in our identity service extension the only 1 i turned off was the required non alphanumeric
              // if u did wan to use a weak passowrd then u would need to turn off all of the different options just stick what we have 
               
                 //changes afte identity
                 await userManager.AddToRoleAsync(user, "Member");// we will out the user into the member role
                 //& we r not checking the results of what 's gonn aon inside here bythe way this is just our seed method 
    //not gonna check the result of this & if we dont get the ri8 result then we r gonna do smthing bcoz we only run this method once
    //& only when our DB is clean 7 we can physically check to make sure that the dat is inside there before we move forward 
    // it;s not smthing that our users is interacting with so i;m skiping the checking of the results for this particular methods 
    //u wont typically see these users into the DB & give them a password either bcoz we r developing the app we r using data to make sure that our app is fucntional


         }// go  them to our database just as we are doing in our register component
         // we still need passwords for these & then we can go & add them to our databse just as we r doing in our register componenets 
         //we still need passwords for these but we also need to use password, salt & password hash 
        // await context.SaveChangesAsync();// this is our seed method 

        // await context.SaveChangesAsync();// user manager takes care of saving the changes into the database
        // but we do need to go to program class bcoz we r going to need to pass in user manager now 

        //change afte indentity
        var admin = new AppUser// we gonna create & new user for the admin 
        {
            UserName = "admin"// set this lowercase just tobe same as the other uses as well 
        };

        await userManager.CreateAsync(admin, "Pa$$w0rd");// remember, this is just a development  & testing I wouldn't really crates it have been easier with this weak password
        //7 then what we do after we've created admin user is will say 

        await userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"});//thistakes IEnumerable of string as roles & what we could do here
    //obviously these r strings & if u spell any of those wrong or differetlt to what we used up here then u notgonna be added to the rolls & stuff 
    
        } 


    }
}