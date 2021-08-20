using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        
        [HttpPut]//mehtod that we use to update a resource on server
        // we r not going to give this any parameter  [HttpGet("{username}")]//bcoz doesn't conflict with anything inside our Api controller'
        // there r certain things that will conflict we can call it a method whatever we want//<IEnumerable<MemberDto>>> GetUsers()//
       //this isn't particularly relevant but what is relevant is the method we use the parameters that we use in the root & therefore the parameters that we take in our method 
       //if we had another http gets but we used ID instead of username then we would have a conflict then & we would need to change our roots in some way 
       //we would say  [HttpGet("id/{username}")]//differnet from naother root 
       // but if we r using a differnt method then our Http PUt is the only HTTp to be put inside here 
       //always gonna be unique 
       public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)// crate a method// But we dont nee dto sne the object back from this bcoz the theory is that the client has all the data related to the entity we r about to update 
       // so we dont need to return the object back from our API bcoz the client is telling theAPI what its updating 
       // it has everyhting it need s & we dont neeed to return the user object fromthis  
       {// want to do when we update a user 1st thing hold of the user & we also need to get hold of the user's username not trust the user to give us their usernaem 
       // we actually want to get it from what we r authenticating again which is the token 
         var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //inside the controller we have acces to a claims principle of the user now this contain info about their identity 
         // want to do find the claims that matches the name identifier which is the claim that we give the user in that token 
         var user = await _userRepository.GetUserByUsernameAsync(username); //once we have the username
       //FindFirst(ClaimTypes.NameIdentifier)?.Value;// now what this hould give us the user username from the token thatthe API uses to authenticate this user that the user we r going to be updating in htis case & once we have the username we can 
         _mapper.Map(memberUpdateDto, user);//whe we r updating or using this to update an object then whatwe can use this particular method this map method has got 10 differnt overloads plenty of difernt options about what we can passinto this
         // & the oveload that we r going to use allows us to pass in this member updates DTo then we will specify what we r going to amp it ot & 
         //_mapper.Map(memberUpdateDto, user);//this save us manualy mapping between our updating 
         //& our user object bcoz if we didn't use that then we would need to 
         //start giving user dots example given below the
         //user.City = memberUpdateDto.City// we nned to go throug this and this fro all the differnt properties inside tere so we dont need to fdo that if we use automatic 
          // thanks to auto mapper 
          _userRepository.Update(user);//now our object is flagged as being updated by entity fraework whatever happens 
          //even if our user has not been updated by simply adding this flag we guareantee that 
          // we r not going to get an exceptions or error when we come back from updating the user in our database 
           if (await _userRepository.SaveAllAsync()) return NoContent();//return form this method if that succesfully we dont need to send any content back for a request and if this fails then what we can do is just retunr a bad request 
           return BadRequest("Failed to Update user");
       }

    }
}