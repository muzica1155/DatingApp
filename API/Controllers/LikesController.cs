//
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]// obviously this is the functionality that we want to use to be logged in 
    public class LikesController : BaseApiController// derive from our base API controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;
        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)// we r gooing to need to bot the user repository & like the repository
        {
            _likesRepository = likesRepository;
            _userRepository = userRepository;
        }
        
        [HttpPost ("{username}")] // we have to do is give the users a amethod to like anothor user of course & stick with using the username property for less
        //some quotes at the {} bcoz this is goingt o be taken from the root parameter So we r users going to specify or our client is going to specify
        // API likes forward slash username (Todd,jane , lisa ) this is the user that they're going to be liking 
        public async Task<ActionResult> AddLike(string username)// & even though we r technically creating a resuouce on here  we dont need to return 
        //anything back we already know what the primary key of the created entity is going to be & we dont to return that to the client 
    //in this case 
    {
        var sourceUserId = User.GetUserId();// make sure we r authenticated to this controller //our new extension method GetUserId
        var likedUser = await _userRepository.GetUserByUsernameAsync(username);// we need ot get hold of the user that we r liking 
        var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId); //we'll get a hold of the source user as well & we'll save our source
    //await _likesRepository.// this time bcoz we wnat to get our user wit likes & we r going to say source use In this case 
    
        if (likedUser == null) return NotFound(); //lets just add some checks in here to make sure that we dont receive some bad data.// check to see if we have liked user
       //just gonna //(likedUser == null) return// just gonna return a not found we didn't find the user that they wanted to like so we'll
       //

       if (sourceUser.UserName == username) return BadRequest("You cannot like YourSelf");//we also check as well to prevent Users from liking themselves which woulkd be possible 
    // just double check to see if we already have this use like Configured & added to our database so what we need to do is we need to go & check to see if we have this 
    var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);//we r going to allow the users to like another user But not gonna to allow or not gonna to implement to togol here
    // if u wanted to implement that togol then obviously if u do alredy have this link then u //(userLike != null) could do smthing to remove the like & give the uses a toggle option
    //we not going down as faras that in this forcus on learning goal mean many to many relationship 

    if (userLike != null) return BadRequest("You already like this user");
    // wanna do smthing differnt here 

    userLike = new UserLike  //if we dont have 1 then we going to crate 1 
    {
        SourceUserId = sourceUserId,
        LikedUserId = likedUser.Id,
        //that what we need to do thi bcoz we have only got two columns & we r just going to put these inside there 
    };

    sourceUser.LikedUsers.Add(userLike);// add the user like & we'll do this to the source user will
     if (await _userRepository.SaveAllAsync()) return Ok();// go head save our changes
     // we didn't actually add save method in our Like repository so we'll mAKE USE of the user repository for the time being consider temporary
     // what we doing here bcoz now we'have got more than 1 repository We NEED TO THINK ABOUT HOW MANY INSTANCE OF DATA CONTEXT WE HAVE & WHAT WE R DOING HERE BUT
     // BUT we r not gonna to break anything from now  this the better options & all of the difernet entities the entit framework is tracking 
     // we will take a look at the achitecture patern boogie for that its called unito of work if u r familiar with that so that what we'll be implementing later
     //but for now use this to move forward //if (await _userRepository.

     return BadRequest("Failed to like User");// if we doen here & save failed then what we'll do is we'll return a bad request & 
     
    

    }
     [HttpGet] // then we need another method to go & get the user like 

    //  public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLike(string predicate) // we r going to take this as a query string parameter we dont need anything additional in  root parameters here & we'll say public hTasl
//GetUserLike()// mor accutely bcoz it could be either 1 of those & we'll take the string 
        //(string predicate)// what r they looking for is
        
        // after updating the changes in LikesRepository  we r going to update this 
        //instead of predicate use LikesParams
        //([FromQuery]LikesParams likesParams) // we needt do this before 
        //then pass likes programs to our _likesRepository 
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLike([FromQuery]LikesParams likesParams)
        {// 
            //  return await _likesRepository.GetUserLikes(predicate, User.GetUserId());// we do return & we'll say 
             //.GetUserLikes(predicate, UserId);// take the user ID of the currently logged in users will sau 
             //action result doesn't work so well with an interface like enumerable So waht we do Var user

             //lets set the ID 
             likesParams.UserId = User.GetUserId();  

            //  var users = await _likesRepository.GetUserLikes(predicate, User.GetUserId());

            var users = await _likesRepository.GetUserLikes(likesParams);
            //what we can do bcoz we r going to get a paginated response back from this now or a page list we r gonna have acces to the currentPage
            //page title pages etc info inside the users. 

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize,
             users.TotalCount, users.TotalPages);// just make sure that ur matches the info that u r expecting in that pagination hader 
             // & put this in place what we can do is we can go to postman & we can test say what I'll do is who 
             return Ok(users);
        }
    }
}










