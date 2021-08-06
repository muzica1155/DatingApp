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
             var thing = _context.User.Find(-1);//we r looking for smthing that we know for sure is nor going to exist // we r not going to have any users with an ID of minus one & nobody will check thr status of thing 
             if (thing == null) return NotFound();
             return Ok(thing);
        }
        [HttpGet("server-error")]// auth is a root parameter 
        public ActionResult<string> GetServerError()
        {// we dont have any complaints from our controller but what we r returning form this technically we would be an app user 
            var thing = _context.User.Find(-1);//we r going to find smthing that we know doesn't exist // what we want to do is generate an exception from this particular method
            var thingToReturn = thing.ToString();//try to execute a method on smthing & set the thing to a string 
            // thingToReturn// whar we r going to get this is null & when we try & execute a method on method on null such as this//ToString();//we r generating from this is a null reference exception
            return thingToReturn;
        }
        [HttpGet("bad-request")]// auth is a root parameter 
        public ActionResult<string> GetBadrequest()
        {
            return  BadRequest("this was not a good request");
        }
        
        }

    }
