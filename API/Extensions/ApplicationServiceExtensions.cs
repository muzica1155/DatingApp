using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using API.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    
    //when we create an extension method first of all the class itself needs to be static and static menas that we do not need to create a new instance of this class in order to use it
    // 
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)//In order to use this or to extend the IServiceCollections that we are going to be returning we need to use this keyword  
        {   
           services.AddSingleton<PresenceTracker>(); //rather than using AddScroped what we r gonna to do is say services & we r gonna to add a singlton & we r gonna in the presence tracker
          // we locked our dictionary every point so that it could only 1 thing at a time & nobody is gonna to be able to try & access this dictionary twice whilst over users accessing it 
          // so it does come with scalability problems bcoz of the nature of using such a thing as a shared resource // there r alternative options for this this is a way of doing it quite simpley for our particular application just to try the presence & whether or not some1 online after that head back to presenceHub.cs

          
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));//when we strongly type a key or configuration in this way just start this t the top 
            services.AddScoped<ITokenService, TokenService>(); //tell our dependency injection container what is his lifetime 
            //AddSingleton //singleton is vcreated or instantiated is created and it doesnt stop unstil our applications stops it countinue using resources not really appropriate for smthing like service but we gonna make atoken 
            //once token is crated we donot need aaround anymore 
            //AddScoped //add scoped is scoped to the lifetime of the http request in this case we are using this in API controller when request comes in ans service injected in that particular controller and that new intence of the services 
            //when the request is finished with the services it disposed this one is going to use all the time 
            //ADDTransient//whic is useful only Always means that services is going to be created and destroyed  as soon as method is finished and this one is normally considered not quite right for an http request this not APPROpriate  
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();//this is gonna take care of our repositories when we inject this into a controller then it's 
            //gonna have a instant So the data context none of our repositories well they r gonna to be using the data context that's injected into the unit of work 
            //& thats the idea we pass that same instance the single instance to each of our repositories & also do go to each repositories
            //changes UNitOdWork replace
            // services.AddScoped<IMessageRepository, MessageRepository>();///next implementation methods for this bcoz we got a big MessageDto we r going to use automapper 
            
            services.AddScoped<LogUserActivity>();

            // services.AddScoped<ILikesRepository, LikesRepository>();//changes UNitOdWork & replace 

            // services.AddScoped<IUserRepository, UserRepository>(); //add service for our repository services at scoped//changes UNitOdWork replace 

            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly); //auto mapper to find those profile the crete maps that we creted map inside this class and thats the configuration set up for automator
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));

            });
            return services;
            
        }
    
    }
}

