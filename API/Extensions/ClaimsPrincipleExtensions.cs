//to simplify the process we use extension method bcoz to make it bit easier & cleaer fro us to go & get the user name from a user claims principle file
//
using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {/// ClaimsPrincipal user)//reason this is claims principle extension if we take a look at the UserController and take a lookat one of our earlier metods then this is what we get from our controller inside our context of our controller
    //we got access to thisuser claims principle 
        public static string GetUsername(this ClaimsPrincipal user)
        {//use GetUsername on the user rather than typing all of this out
            // return user.FindFirst(ClaimTypes.NameIdentifier)?.Value; //during action filter chnges //at the moment our application r going to be broken bcoz this is not now our username but
               // bcoz we  thougt ahead & we created this extension method we need to change this in 1 place 
               return user.FindFirst(ClaimTypes.Name)?.Value; //Name type name for the username & this represents, even though it's called a slightly differnt thing this represents the unique name property that we set inside our token 
             // to make it easy add another extension method inside to get the userId 

        }

        public static int GetUserId(this ClaimsPrincipal user) 
        {
            // return user.FindFirst(ClaimTypes.NameIdentifier)?.Value; // we get an error here bcoz this is a string & we need tpo convert this into an integer that 
               return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);// we r going to return the interger of the user's ID. then take a look at Log user activity method
        }
        
    }
}