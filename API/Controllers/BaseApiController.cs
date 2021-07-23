using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]//controller some attributes
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase // we are inheriting from the controller //automatically validate the parameters that we pass up to an api endpoint based on the validations that we set 
    {
        
    }
}