using System;
using System.Collections.Generic;
using API.Extensions;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }//DTO Data transfer Object
        //when we return appuser properties to a appuer to a client user 
        //another reason to use DTO hindinf certain porperties that maps directly to our database 
        // we can flaten object or we got nested object inside our code and also we lokk relationship we could have circular references between an entity to another cause circuler reference excuptions 
        //main use of dto to our acount controller so we can receive the properties inside the object 

        public byte[] PasswordHash { get; set; } //byte[] arays 

        public byte[] PasswordSalt { get; set; }
        
        public DateTime DateOfBirth { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime LastActive { get; set; } = DateTime.Now;

        public string Gender { get; set; }

        public string Introdution { get; set; }

        public string LookingFor { get; set; } 

        public string Interests { get; set; }///specify their interest 

        public string City { get; set; }

        public string Country { get; set; }

        public ICollection<Photo> Photos { get; set; }//type of relationship called on one to many one use can have many photo
   ///thats what gonna return when eager loading 
        //method in our entity file for age of the customer 
        //property
        
        // public int GetAge() // this were remove it were had nice idea that didn't have desired effect 
        //  { 
        //      return DateOfBirth.CalculateAge();// extend the functionality of the dattime clas so that we can calculate the age based on this date of birth 
        //      //now u cant get somebody age based on the date of birth by our entity 
        //      }
        public ICollection<UserLike> LikedByUsers { get; set; }// give name to under that works bcos these kind of relationships they can be smthimes a bit tricky
        // to get ur head around like their follewers & following relationships that u see in twitter bcoz this is self referencing 
        //we need a way to identify what these collections means
        // this is the list of users that like the currently logged in user & this is the list of users that r currently logged in user has liked 
        //that's the way this relationship  goingt to work 
        // also need to do go head  & configure these entities in DataContext.cs
        public ICollection<UserLike> LikedUsers { get; set; }

        


    

        
    }
}