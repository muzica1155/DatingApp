using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
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




            using var hmac = new HMACSHA512(); //this is going to provide us in hashing algorithm gonna use to create a password hash for
            //when we finished with HMACSHA512();it gonna dispose correctly anytime we are using class with using statement it gonna call a method inside the class called dispose 
            //any class that uses dispose method will implement something called the i disposeable interface 
            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),//specify username and store in lowercase
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),//hmac.ComputeHash method which is expecting byte array i order to get byte array we need to execute another method called getbyte then it runs to exception bcoz get byte expect smthing yo convert into byte array but it cant convert null into bytearray 
                PasswordSalt = hmac.Key
            };
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

        }

        [HttpPost("login")]//new endpoin for login//sending the values in the body of request 
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)///for now return the appuser to refect the successfully login 
        {
            var user = await _context.User
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);
            if (user == null) return Unauthorized("Invalid username");
            //how do we find our user in DB use HMac5.2 because we need to reverse the what we did when we registered 
            //this time we need to calculate the computed hash of the password using the password salt so we had to compare with them
            using var hmac = new HMACSHA512(user.PasswordSalt);//when we create new insistent of this class the 1st constractor create a randomly generate keyword 
            //key which we r passing is salt that what we restoring in database because this going to give sam ecomputed hash of the password because we are giving it the same key that we used when we created password hash in a 1st place 
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            //(loginDto.Password));//if password is same when we created orginally hash version of the password with the same passwordSalt than this should be identical if indentical it can login 
            //if they are not indentical in incorrect password we will return unauthorized 
            for (int i = 0; i < computedHash.Length; i++)
            //this is bytearray loop over each element in this array 
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
             return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)// we are going to get error message because we haven't specify configuraiton  property that we add for our token key //config["TokenKey"]
            };
            // return user;// we have to change this & create a Anothe DTO
            //before edit change the AppUser from ActionResult function
        }


        private async Task<bool> UserExists(string username)
        {
            return await _context.User.AnyAsync(x => x.UserName == username.ToLower());//pass this an expression to check if any username exists or matches the UserName
            //we will use expression To lower thAT ALWAYS gonna compare like for like when we register a user 
            //we also gonna convert username whatever they enter into lowercases as well

        }
    }
}