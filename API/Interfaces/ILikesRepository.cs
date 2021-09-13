// we r going to create 3 methods that we r going to support we r going to get an individual like we r going to get a user with their liked & include them 
//& also we'l have a method to go & get the likes for a user whether they user Loveligt or hulamin line buying 
// waht we r going to return from this is will return a DTA & we'll use this just to select the properties we r interested in for what we return here so 
// we create a new class inside the details & we'll just call this likeDTO
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);//crate task thats gonna return <UserLike> called as just GetUserLike & we get this by its primary key 
        //we'll use the sourceUserID // thi is enough infi to go & get a specific user like //((that 1 method ))

        Task<AppUser> GetUserWithLikes(int userId);
        // Task<IEnumerable<LikeDto>> GetUserLikes(string predicates, int userId);// we return another task that's going to return an innumerable return the type of LikeDto
        // we take 2 bits of info in here we'll take a string of the predicates As in what r we looking for herer we looking for a list of users 
        //that have been liked or liked by & will also take in the end of the user right hee as well bcoz we r going to get this for a specific user 
    // then we can do create our implementations class for this Created a bew class in Data folder 

    Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams); // instead of returning the IEnumerable here we want to return 
    //the page list // instead of passing the strng & id we say LikesParams & likesParams that we passing in to this particular method 
    // then we go LikesRepository
    }
}