//adding functionallity to presist the updates to our API say inside the API inside our users controller
//add a new method to go ahead & do this bcoz we r going to be receiving data from the user we need to add a DTO
//we can use as a parameter for our update method. 
//inside here we r not going to let the user update much in their profile what we r going to ask them or offer them the ability to update is the intro
namespace API.DTOs
{
    public class MemberUpdateDto
    {
      public string Introdution { get; set; }  

      public string LookingFor { get; set; } 

      public string Interests { get; set; }

      public string City { get; set; }

      public string Country { get; set; }
      //bcoz this is info that we r never going to ask them for when they initially register 
      //we r not going to validate anu of thses properties blanks fine in
      //bcoz it a DTO & we r going to want to map this into our user entity 
    }
}