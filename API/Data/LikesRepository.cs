// inside we r going to obviously implement ourlikes repository Interface 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;
        // ! more task we need to go to our app service extension so that we can actually make use of this service in our controller 
        public LikesRepository(DataContext context)// we gonna inject our dataContext inside here call it context & we intitialize the 
        {
            _context = context;
        }


        //take a look at now is the 3 methods that we need to support inside this repository & we'll start with the easisest 1 they get user like 


        public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, likedUserId);// user like al
            // that's enough to go & find the individual like bcoz these 2 things//(sourceUserId, likedUserId)// make upp our primary key of this particular entity 
        }

        // public async Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)// we takng a predicate here so depending on which list ther r looking for 
        // we'll ask him if statement & conditions inside here to return slightly differnt body of a list & we r also taking a user ID as well as taht we can use that to compare against our likes table
        // so when we go a7 get our user likes we r going to go & get a list of users that user has liked & in that case, //int userId)// is goingt to be the sourc user 
     //id we want to go the other way & find out which users have liked the currently logged in user then the user ID is going to be the right hand side table 
     //in the liked user ID & that would give us the opposite list 
        //

        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        // what we r returning is PageList// what we r going to return from this now is page list we also create the instance of that class at the same time 
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();//we r gonna add 2 querable to help us out here 
            //User.OrderBy(u => u.UserName // we add OrderBy & will say u guys to u .username //specify as AsQueryable// going to building up our 
            //queries inside here & but we r also gonna do add var 
            var likes = _context.Likes.AsQueryable();// we r making 2 queries here but we r not really we r going to be joing these inside query &
            //letting entity framework out the joint query that's needed to be made bcoz what we still want to return is a list of users 
            // but we r going to select just the properties we need in that likeDto Lets take a looks using if statement to check
            
            // if (predicate == "liked") //use if statement for consitional here bcoz its complicated 
            // 1 going to be predicate & 1 of them's gonna called liked this is the users that r currently locked in user has liked
            if (likesParams.Predicate == "liked") 
            {
                // likes = likes.Where(like => like.SourceUserId == userId);//specify that our likes is equal to likes .Where
         // even making a effort here to specify what these r in our parameters here so that we can easily identity what's going on 

         likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.LikedUser);// what this give sus is our users from our likes table & our like table is an appUser  we r //like.LikedUser)
                // & selectinf that & we r passing it into our users query   
            }//AND NOW TAKE CARE OF THE ANOTHERWAY

            // if (predicate == "likedBy") 
    // then we r effectively going to similar copy & paste we super careful we want to go other aey this time 

               if (likesParams.Predicate == "likedBy") 
            {

                // likes = likes.Where(like => like.LikedUserId == userId);

                likes = likes.Where(like => like.LikedUserId == likesParams.UserId);
          
                users = likes.Select(like => like.SourceUser);//this should give us the list of users that have liked the currently logged in user 
        //& then what we can do from this is we can return & we'll say await & we'll say uses 
            }

            // return await users.Select(user => new LikeDto // & we'll project this into our likeDto we use select we wont use Autmapper for this we use just a manual selct 
            //statement so that we can project directly into our like Dto & will say we r selecting from our users 
            var likedUsers = users.Select(user => new LikeDto 
            //after changing from IEnumerable// we r not going to awaiting this & we say var linked users 
            //REMOVE THE ToListAsync();
            //now justleft is to return a page list from this //GetUserLikes//
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City,
                Id = user.Id

            });
            // .ToListAsync();//not too much typing particular mapping bcoz there not that many propertied & what we would end up doing if we were using automapper ois probably 
            //typing the same amount of code to configure the mapping for less 
            // now thi is in place let go to API controller 
            return await PagedList<LikeDto>
            .CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);//CreateAsync()// our source in this case is going to be likedUsers
            // which is an IQueryable of the likeDto so we pass in likedUsers& instead of string //(string predicate, int userId)// 
            // we changed later on we have to update everywhere after updating error//(likedUsers,)
            //now we have access to the likesParams page number & also likeParams.PageSize
            // // we crated pagelist instance now we can go to our likescontroller
            
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users// where we just get the list of users that this user has liked & that's what we r going to return from
            //this & 
            .Include(x => x.LikedUsers)
            .FirstOrDefaultAsync(x => x.Id == userId);// getting a user with their collection of links & what we r going to doing when they add alike is er r going
            //to ge adding it ot the user that we return from here 
        }
    }
}