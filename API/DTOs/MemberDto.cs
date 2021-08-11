using System;
using System.Collections.Generic;

namespace API.DTOs
{
    public class MemberDto
    {
         public int Id { get; set; }
        public string Username { get; set; }//DTO Data transfer Object
        //when we return appuser properties to a appuer to a client user 
        //another reason to use DTO hindinf certain porperties that maps directly to our database 
        // we can flaten object or we got nested object inside our code and also we lokk relationship we could have circular references between an entity to another cause circuler reference excuptions 
        //main use of dto to our acount controller so we can receive the properties inside the object 
        public string PhotoUrl { get; set; }// use this property to use as the main photo that we send back for a user 
        
        
        public int Age { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; } 

        public DateTime LastActive { get; set; } 

        public string Gender { get; set; }

        public string Introdution { get; set; }

        public string LookingFor { get; set; } 

        public string Interests { get; set; }///specify their interest 

        public string City { get; set; }

        public string Country { get; set; }

        public ICollection<PhotoDto> Photos { get; set; }
    }
}