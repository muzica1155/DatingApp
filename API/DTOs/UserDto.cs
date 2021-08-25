namespace API.DTOs
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string PhotoUrl { get; set; }//main photo
        //going to do return the user's main photo along with their user object and
        //do is return the users photo with their user object in here 
    }
}