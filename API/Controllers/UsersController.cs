using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// using System.Linq;

namespace API.Controllers
{
    [ApiController]//controller some attributes
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }
            [HttpGet]   // add end points here to get all the user and the specific user 
            public async Task<ActionResult<IEnumerable<AppUser>>>GetUsers()
            {
                return await _context.User.ToListAsync();//returning ToListAsync() users Asynchronously 
                //when a true request goes to database this code pauses it derived to a task and goes and makes the query to the database 
                //when this tasks gets back out of the task and we do that using await key word 
                 

            }

            // public ActionResult<IEnumerable<AppUser>> GetUsers()/// specify type pof thing that we returned inside the result that we send back to the client
            // //IEnumerable allows us to simple iterations over a collections of a specify type instead of that type
            // //we can use list that will return list of users list also offers methods
            // //search, sort amd manupulate list 
            // //specify get users from a database to the list 
            // {
            //   return _context.User.ToList();  //thread which are handing the request is currently blocked unstill the database request is fulfiled
              
            // } // this is called as synchronous code
            // //api/users/3
            // [HttpGet ("{id}")]   //specify root parameters if somebody its this end points 
            // public ActionResult<AppUser> GetUsers(int id)
            
            // { // there is no need to declare a variable if we are not doing anything with it 
            //    return _context.User.Find(id); 
            // //   var user = _context.User.Find(id);   
            // //   return user;//error on user remove IEmumrable
            // }
            public async Task<ActionResult<AppUser>> GetUsers(int id)
            
            { 
               return await _context.User.FindAsync(id); 
            
            }
    
    }
}