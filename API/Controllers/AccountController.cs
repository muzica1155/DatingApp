using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        // private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        // public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        //changes after Identity 
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            // _context = context;// changes after identity 
        }
        [HttpPost("register")]//and we use post if we want to add new resource through our api endpoint
        //when we send httpPost we can send that data in the body of the request gonna we receive 
        //by our api controller what apicontroller does for us it automatically binds to any parameters
        //that finds in the parameters of our method 
        public async Task<ActionResult<UserDto>> Register(RegisterDTo registerDto)//validations//API controller gonna check this for us 7 send automatically if validations fails
        // public async Task<ActionResult<AppUser>> Register(string username, string password)//After debugging //we dont the username and password counld not find by our register method reason for this is we do send something in the body of request we have to send by an object  
        // public async Task<ActionResult<AppUser>> Register([From]string username, string password)
        //if we didn't have API controller attributes then what we need to do add another attributes to the parameters
        //parameters and tell where we are getting the information from and '
        //with differnt option from where we get data to http post request or any request that we can get any body of the request from
        //because we are using APi Controller attributes we do not need to specify this parameters we can relie on API controller being smart enough to pick around where it is coming from and
        {
            //check whether user enter same name or not
            //we cant get access to this particular property or list object simply because we are using ActionResult
            //when we use actionResult we are able to return different http states scope as a reponse to it 
            //
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken ");
            // if(!ModelState.IsValid) return BadRequest("ModelState");
            //{registeration form }
            var user = _mapper.Map<AppUser>(registerDto);// gonna map from our registeredDTO into our appuser & will also keep the method in place to make the username 




            // removed after indentities 
            // using var hmac = new HMACSHA512(); //this is going to provide us in hashing algorithm gonna use to create a password hash for
            //when we finished with HMACSHA512();it gonna dispose correctly anytime we are using class with using statement it gonna call a method inside the class called dispose 
            //any class that uses dispose method will implement something called the i disposeable interface 
            // var user = new AppUser  // remove the creation of the new user here 
            // {
            //     UserName = registerDto.Username.ToLower(),//specify username and store in lowercase
            //     PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),//hmac.ComputeHash method which is expecting byte array i order to get byte array we need to execute another method called getbyte then it runs to exception bcoz get byte expect smthing yo convert into byte array but it cant convert null into bytearray 
            //     PasswordSalt = hmac.Key
            // };
            user.UserName = registerDto.Username.ToLower();//specify username and store in lowercase

            // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));//hmac.ComputeHash method which is expecting byte array i order to get byte array we need to execute another method called getbyte then it runs to exception bcoz get byte expect smthing yo convert into byte array but it cant convert null into bytearray 
            // user.PasswordSalt = hmac.Key; // gonna remove the hashing after identity

            // _context.Users.Add(user);
            // await _context.SaveChangesAsync();// comment afte identity 
            
            var result = await _userManager.CreateAsync(user, registerDto.Password);//& this both create song use saves the changes into the database but we'll check to see if this has succeeded & we'll say 

            if (!result.Succeeded) return BadRequest(result.Errors);//& then we still just return UserDto

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");////add the user into member role as well /the role that we r gonna put them in & we r gonna to put any newly registered user into the role 

            if (!roleResult.Succeeded) return BadRequest(result.Errors);//we'll check & see if the result is successfully
            // in parameters we will do same result.Errors & that what we pass back inot our budget request 
            //now when we register a user they'r automataically gonna go into the member role

            // do couple of checks 1st of all set up smthing really simple to make sure our roles r working Head over userController


            return new UserDto
            {
                Username = user.UserName,
                // Token = _tokenService.CreateToken(user),//changes afte identity
                Token = await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,//we r also going to return 
                Gender = user.Gender,
            };

        }

        [HttpPost("login")]//new endpoin for login//sending the values in the body of request 
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)///for now return the appuser to refect the successfully login 
        {
            // var user = await _context.Users// we still get our access to our Users table

            var user = await _userManager.Users// but we get accesss to our users table we get it by our userManager

            .Include(p => p.Photos)// now we r going to have some photos when we try 7 execute our login method & this source "Photos" is no longer going to be empty if the user doesn't have a photo that will not cause anexception here 
            //bcoz it's simply going to reurn nullBut if it doesn't have anu photos to work with that when we see the exception 
            // .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);//not converting the username to lowercase

            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());//converting the username to lowercase
            if (user == null) return Unauthorized("Invalid username");// if we dont find the user then we r gonna return unauthorized 
            //how do we find our user in DB use HMac5.2 because we need to reverse the what we did when we registered 
            //this time we need to calculate the computed hash of the password using the password salt so we had to compare with them

            //user period after _signInManager u get options inside how we sign in a particular user we gonna use check password & sign in Async 
            var result = await _signInManager
            .CheckPasswordSignInAsync(user, loginDto.Password, false);//need to use our SignInManager ti actually sign in the users 
            //& this takes 3 parameters takes the user object that we have, takes password which is gonna to be in login DTO & then we also provide
          //a boolean to say if we want to lock out the user on failure we r not gonna to do that we say that false
          // pass our user object that will pass in the login password & pass in the loginDto.password then false to say that we r ot gonna lock the user if they get it wrong & MOVE THAT DOWN 
            
            if(!result.Succeeded) return Unauthorized();// we check to see if the result has succeeded 
        //as they probably have got their passwords wrong at this point & the reest of this also stays the same 


            // removed after indetity 
            //  using var hmac = new HMACSHA512(user.PasswordSalt);//when we create new insistent of this class the 1st constractor create a randomly generate keyword 

            // //key which we r passing is salt that what we restoring in database because this going to give sam ecomputed hash of the password because we are giving it the same key that we used when we created password hash in a 1st place 
            // var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            // //(loginDto.Password));//if password is same when we created orginally hash version of the password with the same passwordSalt than this should be identical if indentical it can login 
            // //if they are not indentical in incorrect password we will return unauthorized 
            // for (int i = 0; i < computedHash.Length; i++)
            // //this is bytearray loop over each element in this array 
            // {
            //     if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            // } // removed after indentity


            return new UserDto//this is where we r creating our new user 
            {
                Username = user.UserName,// Source Is our Photos
                // Token = _tokenService.CreateToken(user),//commented after changes identity
                // we are going to get error message because we haven't specify configuraiton  property that we add for our token key //config["TokenKey"]
                Token = await _tokenService.CreateToken(user),
                
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,//in logging method we can also return the phot URL 
                                                                          //get Url property from this even though that they have registered it doesn't mean they have got a photo 
                                                                          //look at he optional property there So the photo u are on will be no '
                KnownAs = user.KnownAs,
                // bcoz we'll make use of that inside our Nav Bar rather tha using the user's username 

                Gender = user.Gender, //update our dto do the same for the register method 
            };
            // return user;// we have to change this & create a Anothe DTO
            //before edit change the AppUser from ActionResult function
            //user.Photos// why would this user object not have any photos while our photos is our related entity ?
            //what we r get for user //inside our account controller we r not using a repository here 
            //what we r doing is we r injecting the data context into this now 
            // we r not going to change this policy//we r not go & add our user repository into this 
            //public AccountController(DataContext context, ITokenService tokenService)
            //this account control is goign to be updated later on when we use identity 
            //but what wr r not doing is returning the photos with this 
            //What we r not doing is returning the photos with this 
            //.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);
            //
        }


        private async Task<bool> UserExists(string username)
        {
            // return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());//pass this an expression to check if any username exists or matches the UserName
            //we will use expression To lower thAT ALWAYS gonna compare like for like when we register a user 
            //we also gonna convert username whatever they enter into lowercases as well
            
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}