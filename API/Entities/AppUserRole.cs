// inside here what we r gonna do gonna specify the joint enities that we need for this so we can have a properly appuser 
//
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{ // what we did by adding these classes 7 changing a few things inside here We do have some errors that we neec to take a look at 
    public class AppUserRole : IdentityUserRole<int>
    {
        public AppUser User { get; set; }

        public AppRole  Role { get; set; }
    }
}