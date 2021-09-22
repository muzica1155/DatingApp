using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{ //the purpose of the controller is to return errors so that we can see what we get back from various response and our app 
    public class BuggyController : BaseApiController
    { //we r going to generate several differnt method that are all going to return kinds of response that r not successful

        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }
        [Authorize]//authorize attributes // purpose of this is to test our 401
        [HttpGet("auth")]// auth is a root parameter 
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }
        [HttpGet("not-found ")]// auth is a root parameter 
        public ActionResult<AppUser> GetNotFound()
        {
             var thing = _context.Users.Find(-1);//we r looking for smthing that we know for sure is nor going to exist // we r not going to have any users with an ID of minus one & nobody will check thr status of thing 
             if (thing == null) return NotFound();
             return Ok(thing);
        }
        [HttpGet("server-error")]// auth is a root parameter 
        public ActionResult<string> GetServerError()
        {// we dont have any complaints from our controller but what we r returning form this technically we would be an app user 
            var thing = _context.Users.Find(-1);//we r going to find smthing that we know doesn't exist // what we want to do is generate an exception from this particular method
            var thingToReturn = thing.ToString();//try to execute a method on smthing & set the thing to a string 
            // thingToReturn// whar we r going to get this is null & when we try & execute a method on method on null such as this//ToString();//we r generating from this is a null reference exception
            return thingToReturn;
        }
        [HttpGet("bad-request")]// auth is a root parameter 
        public ActionResult<string> GetBadrequest()
        {
            // return  BadRequest("this was not a good request");
            return  BadRequest();//we made here change bcoz when u like a person & like that same person u get OK error bad request but 
            //we get message from other bad request  we forget to send a message that what happens then ?
            // if u see that console after remove the message, then u get anothe responnd we get error object insde here Well that makes us need to add another condition bcoz now we have got three different types of 400 errors 
            //we got our validation error which gives us we compare later on we had we formatted this so we cant see the original response but inside there would be an eror array
            //so we have error & then errors whic would be An array of objects & then ew got all of a 400 which has an error object & then we have got the 1 
            //where we put a message in & that's just a string of text 
            // what we do make an adjustment to our error interceptor so that we get things absolutely correct what we r doing eith thses specific errors
        }
        
        }

    }
