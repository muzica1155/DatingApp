//update this last active property when a user interacts with our API we do have options for this we use an action filter 
//action filters allow us to do smthing even before the request is executing or after the request is executed 
// we r going to implements I action filters or I async action filter 
using System;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {//ActionExecutingContext//we have context of the action that is executing 
            //context.//we get inside there gives us acces to our http context , model state, the result data & our controller 
            //ctionExecutionDelegate next)//we also have hear inside this method as wel as we have next what gonna to hapen next after the action is executed & we can use this property ? 
            //use all this parameters to execute the action & then do smthing after thisis executed 
            //What we'll do is we want to get access to the context after this is executed next return = 
            //the action executed context so we get access to the context before & if we take a look at what we return from this then we get the action executed after
            // so since we want to not do this before the users actually doing whatever they're doing, we're going to wait until they've done that 
            //& we're going to execute & updates this last active property 
            var resultContext = await next();// get hold of the context that we get from the next 
            //inside here we have context after the action has been executed 
            // we r going to end this in a central place so we can put it in 1 place kind of switch it on & then forgot about it 
            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;// check to see if the users authenticated bcoz we dont want to try & execute smthing when we dont have a user to do 
            //check the result context & check the Http context & then user & identity 
            //if the user sent up a token & we've authenticated the user then this is going to be true otherwise false check tis property 1st & we need to make sure
            // the user is not authenticated & if that the case we simply return & do nothing els with tthis action filter 
            // if they r authenticated then what we wnat to do is obviously update that last active property 
            
            // var username = resultContext.HttpContext.User.GetUsername(); //choose our extension method to go ajead & get this bcoz 
             
             //this is still a claims principle & we have an extension method on this 
            //now get access to our repository & to do that inside here we can use the service locator pattern & we actually use this inside
            //our program docs class earlier
            var userId = resultContext.HttpContext.User.GetUserId();//means now we can specify the get user by Id async methos in here & swap user username for userID 
            
            //change unitofwork
            var uow = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();
            // var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
            //change unitofwork

            //GetService<IUserRepository>();//to specify the that we want to get accesss to IuserRepository 

            // var user = await repo.GetUserByUsernameAsync(username);//want to get hold of our user object 
            
            //change unitofwork
            var user = await uow.UserRepository.GetUserByIdAsync(userId);
            // var user = await repo.GetUserByIdAsync(userId);//means now we can specify the get user by Id async methos in here & swap user username for userID 
            //change unitofwork

            // need to modify instead of using this// GetUserByUsernameAsync// we going to use the GetUserByIdAsync method & also able to get userID in here as well //var username = resultContext.HttpContext.//we
            //what we wnat to do in addition to returning the user's username & a token we're going to also set the user Id inside there as well so that we'have got easy access to either the ID or the username when we receive a token // TokenService.cs


            //GetUserByUsernameAsync(username);// we r using here  

            //change unitofwork
            user.LastActive = DateTime.UtcNow;
            // user.LastActive = DateTime.Now;
            //change unitofwork

            //change unitofwork
            await uow.Complete();
            // await repo.SaveAllAsync();// now go ahead save changes 
            //change unitofwork

            // we need to do add this is a service this log user activity so we can make use of it add this in ApplicationServiceExtension

        }
    }
}