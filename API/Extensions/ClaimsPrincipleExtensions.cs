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
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        }
        
    }
}