using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppRole : IdentityRole<int>// going to dwrive for the identity roll calss 
    //we dont need any properties inside here at the moment 
    // we r going to need to want to get a list of roles let the users in Now in order to do this identity doesn't provide us with a way to do this out of the box '
    // this is gonna be another many to many relationship each app user can be in multiple roles 7 each role can contain multiple users 
    //ahead to entities 
    {
        public ICollection<AppUserRole> UserRole { get; set; }// create a collection inside here 

        // do exactly the same thing inour app uer class to complete the realtionship that we r gonna add for these entties & will add 
        
    }
}