// inside here the properties that we want to return we return the end of the user ID so we'll call it int ID
namespace API.DTOs
{
    public class LikeDto
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public int Age { get; set; }// end we rreturn the end of their age &  
        public string KnownAs { get; set; }//we'll return the string of KnownAs 
        public string PhotoUrl { get; set; }

        public string City { get; set; }

        //now thisis the info we need to create a member card just like this displaying on our member list & the uses we like 
        // we r going to display them just as cards . we'll get the info from this & we'll go back to our !LIKE repository

    }
}