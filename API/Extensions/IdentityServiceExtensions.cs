using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtensions//static class hold the extensions 
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,
         IConfiguration config)//static method 
        {
              services.AddIdentityCore<AppUser>(opt =>
              {
                  opt.Password.RequireNonAlphanumeric = false;//Identity required complex password so if u wanted to configure less & u r want just turn off just as an example
                 // opt.SignIn. // u have other options // we r not gonna turn off any other options just gonna keep the defaults for less 
                 
              }
              )//here we configure identity add another service
              .AddRoles<AppRole>()//& we gona do start chainning on the services & we want to use roles in app
                 .AddRoleManager<RoleManager<AppRole>>()//be careful with RoleManager bcoz we need to pass in 2 types inside her it's 
                 //extremely common for me to see a question from a student where i live configured the road manager easy to make misstakes 
                 .AddSignInManager<SignInManager<AppUser>>()//be careful with
                 .AddRoleValidator<RoleValidator<AppRole>>()//be careful
                 .AddEntityFrameworkStores<DataContext>();// tis is the 1 we need so that it sets up our databse with all of the tables we need to craete the dot net identity & we need to bring indata context inside here as well
   // be careful u need to be exact & each of these services also has their own type such as well validator sign manager & role manager & then furthe adding new migration

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//added authentication scheme in parameters
            //chain on some configuration 
                .AddJwtBearer(options => 
                //changes during Signalr 
                //for signalr we cna always use a query string whereAs for our Api controller it's just gonna use the authentication header as we've been using so far

                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            //this 2 are flags do just that 
                            ValidateIssuerSigningKey = true,//our server gonna sign our token we need to tell it to validate this token is correct
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])), /// then we have give the issuer the sign in key
                            ValidateIssuer = false, ///Issuer of the token is our API server
                            ValidateAudience = false, ///Audience of the token is our augular App
                            //we can authenticate our users with a valid token 
                        };
                        //changes during SignalR
                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>  //tarhet the on message received & we'll pass through the context & this is gonna http context
                            //& add an arroe & 
                            {
                                var accessToken = context.Request.Query["access_token"];//[""]//what we get from this the accesstoken now 
                                //this needs to be specific bcoz signalR by dafault will send up our token As a query string with the key of ["access_token"]
                            
                                var path = context.HttpContext.Request.Path;// check the path of this request wher's it coming to ?bcoz we only want to do this for signalr specify path
                            
                                if (!string.IsNullOrEmpty(accessToken) && 
                                path.StartsWithSegments("/hubs"))// we want to see if we have an access token & see if we've got our JWT token 
                                // if the string is not empty So let the not operator & then say & pass through our access token &&//& we want to check the path 
                                //("/hubs"));//this needs to match what we used inside our startup class 
                                { context.Token = accessToken; }
                                return Task.CompletedTask;
                            }
                        };// all this allow our client to send up the token as a query string But when we do this we also need to suply smthing 
                        //else to our couse configuration bcoz what we also need to allow here & .AllowCredentials()in Startup class we need to supply lists when we r using signalr due to the way that we now send up our access token 
            //or our token So that takes care of the basic services & configuration 
                    });

              services.AddAuthorization(opt => 
              {
                opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));//policy have different options here various thing we can do with our policy we r gonna use required Role
                opt.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));
              });//we gonna add authorization//() provide this with options 
              //admin role required in the admin role 
                    return services; }}}


        