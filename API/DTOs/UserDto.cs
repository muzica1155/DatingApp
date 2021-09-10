namespace API.DTOs
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string PhotoUrl { get; set; }//main photo
        //going to do return the user's main photo along with their user object and
        //do is return the users photo with their user object in here 

        public string KnownAs { get; set; }

        public string Gender { get; set; }//want to do deafult value for gender & we r going to replicate this on the client site & RATHER MAKING A SPECIFIC REQUEST TO go & get the user's gender 
        //when we do this we update our account controller 1st update our UserDto 
        //and send back another bit of info about the user when we log in send back the gender as well as other properties  here 
        //save us from making an API call simply to find out what gender the user is or the currently logged in user bcoz we r always going to have that info available 
        //
        
    }
}