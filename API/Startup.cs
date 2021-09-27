using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Middleware;
using API.Services;
using API.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)//acces this configurations IConfiguration config is injected into the class when it constractored  
        {   ///when we can have access to the configurations when we use this file 
            _config = config;


        }//Do we need an interface for our taking services ?//we dont 
        //we could just create te token service on its own and this would function just as it before
        //the reason for creating interfaces is twofold really 
        //one testing //its very easy to molk an interface we dont need to implement anything in the interface
        //all we need is the signature of the method & then we can mock it behavior when it comes to testing our applications 
        //best practice when we do have inteface they're just available & we can go ahead and test them whenever we want 

        // public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)//dependence injection container 
        {
            // services.AddScoped<ITokenService, TokenService>(); //tell our dependency injection container what is his lifetime 
            // //AddSingleton //singleton is vcreated or instantiated is created and it doesnt stop unstil our applications stops it countinue using resources not really appropriate for smthing like service but we gonna make atoken 
            // //once token is crated we donot need aaround anymore 
            // //AddScoped //add scoped is scoped to the lifetime of the http request in this case we are using this in API controller when request comes in ans service injected in that particular controller and that new intence of the services 
            // //when the request is finished with the services it disposed this one is going to use all the time 
            // //ADDTransient//whic is useful only Always means that services is going to be created and destroyed  as soon as method is finished and this one is normally considered not quite right for an http request this not APPROpriate  
            // services.AddDbContext<DataContext>(options =>
            // {
            //     options.UseSqlite(_config.GetConnectionString("DefaultConnection"));

            // });
            services.AddApplicationServices(_config);

            services.AddControllers();
            services.AddCors();
            services.AddIdentityServices(_config);/// wht extension method is use to save us from typing repeatative code
                 //changes after SignalR
            services.AddSignalR(); // all so need to do tell our rooting about our API or our Hub endpoints
            //
            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//added authentication scheme in parameters
            // //chain on some configuration 
            //     .AddJwtBearer(options => 
            //         {
            //             options.TokenValidationParameters = new TokenValidationParameters
            //             {
            //                 //this 2 are flags do just that 
            //                 ValidateIssuerSigningKey = true,//our server gonna sign our token we need to tell it to validate this token is correct
            //                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"])), /// then we have give the issuer the sign in key
            //                 ValidateIssuer = false, ///Issuer of the token is our API server
            //                 ValidateAudience = false, ///Audience of the token is our augular App
            //                 //we can authenticate our users with a valid token 
            //             };
            //         });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        { 
            app.UseMiddleware<ExceptionMiddleware>();
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();//if we dont have any exception handling inside any of our methods controllers or middleware then its gonna get thrown all te way up to our developer exception page 
            //     app.UseSwagger();
            //     app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            // }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials() // in Startup class we need to supply lists when we r using signalr due to the way that we now send up our access token 
            //or our token 
            .WithOrigins("https://localhost:4200"));

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<PresenceHub>("hubs/presence");//("")//give this a root what route is tis president hub gona to be accessed from ?
                // we can have more than 1 hub 
                // this takes care of setting up our 1st hub what we also need to do is take care of authorization bcoz our hubs r also going to be authenticated
                // we dont want to try & get a username if a user is not authenticated for instance
                endpoints.MapHub<MessageHub>("hubs/message"); // need to add another endpoint 
                // this takes care of when a user connects what we also want to do is take a look at sending a message via this hub & we;ll look next 

            });
        }
    }
}
//extension methods enable us to add methods to existing types without creating a new dereived type or modifying the original type