using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
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
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));//when we strongly type a key or configuration in this way just start this t the top 
            services.AddScoped<ITokenService, TokenService>(); //tell our dependency injection container what is his lifetime 
            //AddSingleton //singleton is vcreated or instantiated is created and it doesnt stop unstil our applications stops it countinue using resources not really appropriate for smthing like service but we gonna make atoken 
            //once token is crated we donot need aaround anymore 
            //AddScoped //add scoped is scoped to the lifetime of the http request in this case we are using this in API controller when request comes in ans service injected in that particular controller and that new intence of the services 
            //when the request is finished with the services it disposed this one is going to use all the time 
            //ADDTransient//whic is useful only Always means that services is going to be created and destroyed  as soon as method is finished and this one is normally considered not quite right for an http request this not APPROpriate  
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();
            services.AddScoped<ILikesRepository, LikesRepository>();
            services.AddScoped<IUserRepository, UserRepository>(); //add service for our repository services at scoped
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly); //auto mapper to find those profile the crete maps that we creted map inside this class and thats the configuration set up for automator
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));

            });
            return services;
            
        }
    
    }
}

