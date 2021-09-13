//we r going to give AppUser to new collections is goingt o be a list of users that they like & another 1 is going to be a list of users that have liked that user
//& in order to accomplish this we need to to crate a joint entity 
// we'll typically combine the name of the 1 entity to the other that we r joining here 
namespace API.Entities
{
    public class UserLike
    {
        public AppUser SourceUser { get; set; }// source user the user that is liking te other user & will give this 

        public int SourceUserId { get; set; }//

        public AppUser LikedUser { get; set; }// this is going to be the linked user & will create an interface 

        public int LikedUserId { get; set; }// then we go to appUser & we add to collection & the open up the app user 

        
    }
}