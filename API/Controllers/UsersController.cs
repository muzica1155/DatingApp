using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        //changes unitofwork
        // private readonly IUserRepository _userRepository;//changes unitofwork
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        
        private readonly IUnitOfWork _unitOfWork;//changes unitofwork
        // public UsersController(IUserRepository userRepository,//changes unitofwork

        public UsersController(IUnitOfWork unitOfWork,
         IMapper mapper, IPhotoService photoService)// we nned to inject imapper interface that we get from automatic inside here 
        {
            _unitOfWork = unitOfWork;//changes unitofwork
            _photoService = photoService;
            _mapper = mapper;
            //changes unitofwork
            // _userRepository = userRepository;//changes unitofwork
            //     _context = context;
        }

        //cahnges identity
        //check to make sure that our roles work //just add authorized attribute & just temporary we gonna say in paramaters
        // [Authorize(Roles = "Admin")]//Admin means Only can access the ful list od users This is temporary jus tot make sure our roles r working or not 

        [HttpGet]   // add end points here to get all the user and the specific user 
                    //ensure that our endpoint are protected wit authentication is at an authorized attr
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)// we r not taking anything in a request at the moment 
        //but what we r going to do is take all of these parameters as a query string & we r going to store that in an object 
 //(UserParams userParams)// bcoz its a query string we need to specify [FromQuery] give our API controller this attribute going to get the user parameters from the query string parameters
//we didn't supply anything in the query string & we have got the user param objects there API got confused 

        //need to do add this info that we get back from our  page list into our response header //GetUsers(UserParams userParams)
        {
//Populate the current user name property into the userParams & set a default propertythat is just the opposite to their current gender if they dont specify anything inside here 
//before we pass our user parameters to get members method we r going say user progrma current username is equal to user & will say get user name 
    
    //change unitofwork 
    //  var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());//change unitofwork //this query here that the 1 that's causing that big long list of text in terminal
     
     // optimization query
        var gender = await _unitOfWork.UserRepository.GetUserGender(User.GetUsername());
     // optimization query
    //   var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());//change unitofwork
    
    //  userParams.CurrentUsername = User.GetUsername();

    //optimization query
    userParams.CurrentUsername = User.GetUsername();// now all this is for is simply to get an gender after this take a look on user repository & create a method specifically just to get users gender
    // userParams.CurrentUsername = user.UserName;//but all we need from this really is the users gender bcoz we dont need the user to go & get the user's username we can just get that from the token using user & get username
    //optimization query

     // add a check 
     if (string.IsNullOrEmpty(userParams.Gender))
           
        //    userParams.Gender = user.Gender == "male" ? "female" : "male";

           // optimization query
           userParams.Gender = gender == "male" ? "female" : "male";//see the terminal with less query not getting unneccesary data from DB
           // optimization query

           //we actually need to get thecurrent user in order to get access to the gender 
           
           //changes unitofwork
           var users = await _unitOfWork.UserRepository.GetMembersAsync(userParams);
            // var users = await _userRepository.GetMembersAsync(userParams);// now our users now thisuses variable is now a page list of typed memeber DTO mean we got our pagination info inside here as well  
           //changes unitofwork

            // var users = await _userRepository.GetUsersAsync();//we are using repository & gwtting hte users from our repository 
            // var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);///Map<MemberDto>();// pass the source object in parameters// we dont just want ot map to our member Dto want map to mappersto also wanto to map
            //IEnumerable of our mumber dto
             //(userParams);// but we r sending this up as an object & query string parameters can also go as individual properties here 
                Response.AddPaginationHeader(users.CurrentPage, users.PageSize,
                 users.TotalCount, users.TotalPages);
                // get access to our response which we have inside our controllers we have always got access to the http request stuff inside here //which is extension method we created
                //in Pareameters takes int currentPage int items so on 
                //bcoz we r in API controller & we r using that API controller attribute, this controller should be smart enough to recognize when we send up query string parameters & going to match them inside here or it should do 
                // Our API controller not that smart Unsuppoerted media error on postman request

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

//Changes after identity 
//for individual Http get
        // [Authorize(Roles = "Member")]// so memebers can get an individual user whereas only theadmin can get a full list of users

        [HttpGet("{username}", Name = "GetUser")]//then //Name = "GetUser")]//then we can use this routes name as one of the parameters in the created up rates so we can specify 
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
            
            //changes unitofwork
            return await _unitOfWork.UserRepository.GetMemberAsync(username);
            // return await _userRepository.GetMemberAsync(username); 
            //unit of work

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
           
            //after deployment
             var username = User.GetUsername(); //inside the controller we have acces to a claims principle of the user now this contain info about their identity 
               //after deployment                                // want to do find the claims that matches the name identifier which is the claim that we give the user in that token 
           //change unitofwork
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername()); //once we have the username
        //  var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername()); 
        //change unitofwork
           
            //    var user = await _userRepository.GetUserByUsernameAsync(username);
           //instead of storing the username in a variable we can just pass that method into our get username
            //FindFirst(ClaimTypes.NameIdentifier)?.Value;// now what this hould give us the user username from the token thatthe API uses to authenticate this user that the user we r going to be updating in htis case & once we have the username we can 
            _mapper.Map(memberUpdateDto, user);//whe we r updating or using this to update an object then whatwe can use this particular method this map method has got 10 differnt overloads plenty of difernt options about what we can passinto this
                                               // & the oveload that we r going to use allows us to pass in this member updates DTo then we will specify what we r going to amp it ot & 
                                               //_mapper.Map(memberUpdateDto, user);//this save us manualy mapping between our updating 
                                               //& our user object bcoz if we didn't use that then we would need to 
                                               //start giving user dots example given below the
                                               //user.City = memberUpdateDto.City// we nned to go throug this and this fro all the differnt properties inside tere so we dont need to fdo that if we use automatic 
                                               // thanks to auto mapper 
            //change unitofwork
            _unitOfWork.UserRepository.Update(user);
            // _userRepository.Update(user);//now our object is flagged as being updated by entity fraework whatever happens 
            //change unitofwork
            
            //even if our user has not been updated by simply adding this flag we guareantee that 
            // we r not going to get an exceptions or error when we come back from updating the user in our database 
            
            //change unitofwork
            if (await _unitOfWork.Complete()) return NoContent();
            // if (await _userRepository.SaveAllAsync()) return NoContent();//return form this method if that succesfully we dont need to send any content back for a request and if this fails then what we can do is just retunr a bad request 
            //change unitofwork
            
            return BadRequest("Failed to Update user");
        }

        [HttpPost("add-photo")]//allow the user to add a new photo. nee http bcoz we r creating a new resource & we give 
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)//we need our client to get a couplr of value from what we returning here due to the logic that we can add we need to 
                                                            //need to know that the client is going to need to know the idea of the photo & also if te photo is the main
                                                            //photo we going to need to return our newly created photo from this & we ll just cal a method & 
        {// we gonna just alloe the user to update file Not gonna take any extra info 
            
            //change unitofwork
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            // var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());// wer getting our user when we do this method this includes our photos we r eagerly loading them in this method & we need to for this  
            //change unitofwork
           
            //now we have our user object on a one line here as well 
            var result = await _photoService.AddPhotoAsync(file);//then we get our result back from the photo service & then we check to see the result if we got an error we r going to return a bad request 
            // if we pass from the error
            //pass file to our add photo async method 

            if (result.Error != null) return BadRequest(result.Error.Message);
          // check to see if this is not equal to now & if it is not equal to now then we have a problem 
          var photo = new Photo//we create a new photo we check to see if te users already got any photos in 
          {
              Url = result.SecureUrl.AbsoluteUri,// we want thesecure URL which doesn't say its deprecated obviouslt same thing 
              // what her we want absolute
              PublicId = result.PublicId
          };

          // if they dont then we r going to set the photo to main 
          if (user.Photos.Count == 0)//we need to check to see if the users got any photo at the moment 
           /// iff it is then we knoe that this is the 1st img that the user uploading and if it is thee 1st photo uploaded then we r going to set this one to Main
           {
               photo.IsMain = true;
           }      
           user.Photos.Add(photo);// then we add the photo 
           
           //change unitofwork
           if (await _unitOfWork.Complete())
        //    if (await _userRepository.SaveAllAsync())// then we return the photo & we return a badrequest if it all goes horribly 
           //change unitofwork
           
           {
            //    return _mapper.Map<PhotoDto>(photo);
           // if that false
           //{how we can use the creator route to generate the correct response or return it to a one created when we add resource to a server think ? where is ourresource going to be loacted }
           //{so our locatuon for getting the photos is ogingot be our user make sure that we return the photo in the body of the request as well }
             return CreatedAtRoute("GetUser", new {UserName = user.UserName}, _mapper.Map<PhotoDto>(photo));// 2nd parameters wa route values
             // now we  rgoing to return 201 to everyone & we r going to return the roots of how to gte the user which contains the photos and we canstill return our photo object 
               //giving server error complaing about got a route for getUser But havn't supplied for the username 
            //    return CreatedAtRoute()//use a differnt overload of created up route our route name take sparameters in the route parameters 

           }
           

           return BadRequest("problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]//when we r updating smthing we want to use httpPut
        public async Task<ActionResult> SetMainPhoto(int photoId)// remember no need to pass the object back 
        {
            //change unitofwork
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            // var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            //change unitofwork
        
        //remember when we r doing this with any of these methods when we r getting our user name from our token this means 
        //we r validating that this is the user that they say they r we can trust the info inside the token no trickery goign on bcoz our servers signed the token the users sent us the token 
        //if they saying their name is lisa the we r going to trust that correct So they r authenticating & we r going to get this user that is authenticating to this method 
        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);  //get the hold of the photo tat matches this specific ID 
        //GetUserByUsernameAsync// remember that this aprticular method that we get from our repository  has an eagerloading property for the user's photos collection 
        //So we do have have access to the photos inside here 
        //FirstOrDefault()// this is not asynchronous bcoz we r already got the user in memory at this point so we r not going to the database now we already done that inside the repository 
        if (photo.IsMain) return BadRequest("This is already your main Photo"); // we allowed to check here to amek sure the user is not trying to set a photo that is main to main 
        //if somehow they managed to do that which we will of course prevent thme from doing on the client 
        
        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);// if they get pass less than what we need to do get hold of the current main photo & make that is main property & set light to false & update this photo that at the passing in And set that on to true 
    //& this will give us the current main photo & i trying to think of occasions wher this could be null 
    //lets check anyway 
    if (currentMain != null) currentMain.IsMain = false; //
      photo.IsMain = true; // also set the phot that we r passing in the one that we r trying to get main wil set 
    // we want to turn off the curretn main & turn on the new 1 that they have just set to that 
   
   //change unitofwork
   if(await _unitOfWork.Complete()) return NoContent();
    // if(await _userRepository.SaveAllAsync()) return NoContent();// we dont need to send anything back in this request 
   //change unitofwork
     
     return BadRequest("Failed to set Main Photo");// just in case we get this far & smthing went wrong then  we will return a bad request & we will 
    }

    [HttpDelete("delete-photo/{photoId}")]//bcoz we r going to be deleting a resource //going to do give this some additional values bcoz we dont want to use the name of our controller less
 //what we ll dogive this a route name & we ll specify delete dash photo is 
 public async Task<ActionResult> DeletePhotoAsync(int photoId)
 {
     //change unitofwork
     var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
    //  var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());//need to do again is go & get our user object 
     //change unitofwork
     
     var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);//can do is go & get the photo// that we r interested in deleting so what well do is well save our photo equals
     if (photo == null) return NotFound();// we do a couple of check inside here
     //check this & see if the photo is null // we couldn't find the photo in the user's photo collection
     if (photo.IsMain) return BadRequest("You cannot delete your main photo");//we'll also chek & check to see if the photo is main if it is 
     if (photo.PublicId != null) //another check bcoz what we want to do want to check to see if we ave a piblic ID for this particular 
     //img bcoz sm of our photos have this property some of them dont & only ones that we need to delete from Cloudinary r the ones that do have a pulbic ID so what 
     //if thr public ID is not equal to null 
     {
         var result = await _photoService.DeletePhotoAsync(photo.PublicId);
         //we should do here actually if we have a problem deleting it from cloudenary then do we want to delete it from our database ?
         //will store the result in a variable & we'll save our result equals 
         //What we get is our deletion results
         if (result.Error != null) return BadRequest(result.Error.Message);//inside the result we justcheck to see what we have available inside here 
     // jus going to return out of our method now this is gonna stop execution when we return & we r going to return a bad request 
      user.Photos.Remove(photo);//we r going to assume that was successful & whteve type of image it is but then remove it from our database
     }// will do more accurately we say use it photos . remove & will pass in the photo
   // all this does is at the track & flag & we r updaing our user at htis point remember bcoz the photo is a related entity on our user 
    
    //change unitofwork
    if (await _unitOfWork.Complete()) return Ok();
    // if (await _userRepository.SaveAllAsync()) return Ok();//what well do use a repository and save all async
    //change unitofwork
    
    return BadRequest("Failed to delete ");//& if we dont have succes with saving this then we going to return a bad request once again 
 }
 //when we delete a resource then we dont need to send anything back to the cliet 
}
}