using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// using System.Linq;

namespace API.Controllers
{
    [Authorize]
    // [ApiController]//controller some attributes
    // [Route("api/[controller]")]
    public class UsersController : BaseApiController //ControllerBase when we inherite from the another c# class we get all of it attributes, methods and properties available in class that derived from that particular class. 
    {
        // private readonly DataContext _context;
        // public UsersController(DataContext context)
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        
        public UsersController(IUserRepository userRepository, IMapper mapper)// we nned to inject imapper interface that we get from automatic inside here 
        {
            _mapper = mapper;
            _userRepository = userRepository;
            //     _context = context;
        }

        [HttpGet]   // add end points here to get all the user and the specific user 
                    //ensure that our endpoint are protected wit authentication is at an authorized attr
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();
            // var users = await _userRepository.GetUsersAsync();//we are using repository & gwtting hte users from our repository 
            // var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);///Map<MemberDto>();// pass the source object in parameters// we dont just want ot map to our member Dto want map to mappersto also wanto to map
            //IEnumerable of our mumber dto

            return Ok(users);//when we use projection we dont actually need to include bcoz the entity framework is going to work out the correct query to join the table & get what we need from the database So this can be more efficient way of doing things
            // return await _userRepository.GetUsersAsync();not gonna work in repository if wrap inside ok rsult
            // return await _context.User.ToListAsync();//returning ToListAsync() users Asynchronously 
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
        // [Authorize]
        // [AllowAnonymous]
        [HttpGet("{username}")]
        // [HttpGet ("{id}")]   //specify root parameters if somebody its this end points 
        // public ActionResult<AppUser> GetUsers(int id)
        // { // there is no need to declare a variable if we are not doing anything with it 
        //    return _context.User.Find(id); 
        // //   var user = _context.User.Find(id);   
        // //   return user;//error on user remove IEmumrable
        // }
        // public async Task<ActionResult<AppUser>> GetUsers(int id)//[AllowAnonymous] for GetUsers
        public async Task<ActionResult<MemberDto>> GetUser(string username)// this method is using to get individual user 
        {

            ///wat we r returning is our memberDTO and also maper is teken care of all of the mapping between our app user and the member 
            // var user = await _userRepository.GetUserByUsernameAsync(username);//we got our entity in memory from this request & then we go in memory we map from one object to another & its very easy to argue that this cannot be efficient 
            return await _userRepository.GetMemberAsync(username);
            //we r returning a member now directly from our repository bcoz thats what this is returning from
            //    return await _context.User.FindAsync(id); 
            // return await _userRepository.GetUserByUsernameAsync(username); //swap the id for a user name in this case 

            // return _mapper.Map<MemberDto>(user); 
        }//surely its better to go just the properties we need from the database and then at the database level passes back DTO rather than getting the entity & then converting it into a DTO (argument good point )


    }
}