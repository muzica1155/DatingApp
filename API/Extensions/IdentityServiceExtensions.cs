using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//added authentication scheme in parameters
            //chain on some configuration 
                .AddJwtBearer(options => 
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
                    });
                    return services;


        }
    }
}