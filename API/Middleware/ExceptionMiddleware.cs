using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{// when we r using or adding middleware into dotnet API we need contructor,  REquestDelegate
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
         private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next,
       
        ILogger<ExceptionMiddleware> logger, IHostEnvironment env) //requestdelegate is what next whats coming up next in themiddleware pipeline 
                                                                                                                    // Ilogger so that we ca still log out exception into the terminal what to display it in our terminal windoe where we r running dotnet run
                                                                                                                    // and also we want o check the environment we r running in are we in production r we in development ( logger, IhostEnvironment)
        {
            _env = env;
            _logger = logger;
            _next = next;

        }
        //middleware required method is async and
        public async Task InvokeAsync(HttpContext context)// bcoz this ia happening in the context of an http request when we ad moddlware we have access to the actual http request that coming in 
        //inside this method using try catch 
        { // we r going to do get a context & simply pass this on  to the next piece of middleware is going to live at the very top of our middleware & anything below let if we have 17 bits of middleware that organa inviked next 
        //at somepoint & if any of them get an exception they going to threo the exception uo & up and until they reach smthing that can handle the exception and
        //bcoz our exception middleware is going to be at the top of that tree then we r going carch the exception inside here
        //
            try 
            {
                await _next(context);
            }
            catch (Exception ex) 
            { 
                _logger.LogError(ex, ex.Message);//if we dont do this our exception is going to be silence in our terminal //not good less info we get 
                context.Response.ContentType = "application/json";//write this exception to our response so we got access to the http context 
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;//effewctively going to be a 500
                var response = _env.IsDevelopment() //we r going to check to see what environment we r running in are we running in development mode
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) // we use iternary operator to say what we r going to do if thisis our development mode
             //? new Api//going to create new API exception using our own API exception class & we will bring in using API errors
             ////, ex.StackTrace?) // just incase this is null we need to add the question mark there & then we can say no strength // we dont want to cause an exception in a exception handling middleware in case this is null we r going to prevent any exception from this by adding that by adding ?
                    : new ApiException(context.Response.StatusCode, "InternalServerError");//in case we r not in development mode we know we r in production mode keep the 500 error we sned back to the
              // we will create some options bcoz what we can is send back this to json by default we want our json response to go back in camil case ceate a option to enable this bcoz we need to serialize this response into json response  
                     var options = new JsonSerializerOptions
                     {PropertyNamingPolicy = JsonNamingPolicy.CamelCase}; // gving property naming policy so// this going to ensure that our response just goes back as a normal Jason formatted response in cAMIL CSE THAT WE r going 
                     var json = JsonSerializer.Serialize(response, options);// serialize the rsponse that we created earlier & pass in some options as in the options r we want this to bein camel case
                     await context.Response.WriteAsync(json);//this is our middleware & what we can do now is we can go back t
            }

        }


}
}