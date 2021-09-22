using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController // derve from our base API controller purnono
    {
        private readonly UserManager<AppUser> _userManager;
        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]//& we dont have this policy yet of course &
        // here we r gonna create 2 methods
        [HttpGet("users-with-roles")] //what we want to return from here is a list of users with their roles & we'l take a look at how we can do that 
        // the user manager doesn't comes with this functionality surprisingly but what we r gonna do is need to use a query to go & get all of the users with their specified roles bcoz 
        //what we want the admin user to be able to do is edita user's roles what we do add a constractor & we can inject in here the user 
        //in !st methos we gonna give root 
        // public ActionResult GetUsersWithRoles()
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users
            .Include(r => r.UserRole)// we ned to get our use of roles related list of user roles 
            .ThenInclude(r => r.Role)//but we also need to include the role itself in there we need to then include this time 
            .OrderBy(u => u.UserName)// we gonna add orderBy expression
            .Select(u => new //new just gonna return anonymous object // we dont have a type for what we were returning here that y we use anonymous object
            {
               u.Id,//for the userID
               Username =u.UserName,//for the
               Roles =u.UserRole.Select(r => r.Role.Name).ToList()//we want to get fro here is roll name 
            })// then we use Select that we can project out of tis 
            .ToListAsync();// to remove error bcoz we gonna projects in here we r gonna select what we r lookin for inside our users 
            // return Ok("Only admin can see this ");// obviously testing our authorization here 

            // we gonna get an object back with the userId, username& roles that they r in & then we r sending that to a list 

            return  Ok(users);// then we'll return users we r not gonna paginate these result but we r gonna focus on role managerment 
        }


        [HttpPost("edit-roles/{username}")]// -roles & take it as route parameters {username}
        // getting the username from every parameters of 

        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)//(string username, //then wha to do pass up rols as a query string so we r gonna get the username from the root parameter 
        //but then we also want to do smthing with query string as well  we specify from query bcoz this will definitely confuse our API controller
// if we dont do it then gonna take a string of rolls we r gonna get from query string 
         {
             var selectedRoles = roles.Split(",").ToArray(); // these r gonna come up as a comma separated list & we gonna slits then by a comma & then pass to an ARrey
   // remember we r gonna get these from our query string // roles.Split// we gonna have a member comma admin or member comma moderator etc then split them to an array inside here 
             var user = await _userManager.FindByNameAsync(username);

             if (user == null) return NotFound("Could not find user ");//what we wnat ot do check to make sure that we hav a 
//otherwise we'll get an exception bcoiz if we try & executes this method //GetRolesAsync(user);//on null then thisis gonna return an exception to us 

             var userRoles = await _userManager.GetRolesAsync(user); // pass in our user which is gonna give us list of roles for the particular user

             // we want to take a look of the list of roles & the user to the roles unless they r already in tha particular role
             var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));// userRoles  which is to use the roles the user is currently in

             if (!result.Succeeded) return BadRequest("Failed to add to roles ");//then ew hav eto check if that succeeded or not 

             result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));// now useRoles & the user currently in at this point//var userRoles//
             //

             if (!result.Succeeded) return BadRequest("Failed to remove from roles");// one agian we check the status of theresult so we'll use results succed with operator 
              
              return Ok(await _userManager.GetRolesAsync(user));//get roles for htis specific user so that we return them as well 
         }

        [Authorize(Policy = "ModeratePhotoRole")]//& we dont have this policy yet of course &
        // here we r gonna create 2 methods
        [HttpGet("photos-to-moderate")] //in !st methos we gonna give root 
        public ActionResult GetPhotosForModeration()
        {
            return Ok("Admins or moderators can see this ");// obviously testing our authorization here 
        }
        // after finishing we r gonna go & set up these policy "RequiredAdminRole")]

    }
}