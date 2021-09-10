//where our controllers  r all deriving from so we'll go to our base API controller 
using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]//add this attribute to this controller & then all of our controllers & therefore all of the actions inside of those controllers r going to be making use of this action filter 
    //we do & specify that we wnat to use a service filter & then we give it a type of 
    [ApiController]//controller some attributes
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase // we are inheriting from the controller //automatically validate the parameters that we pass up to an api endpoint based on the validations that we set 
    {
        
    }
}