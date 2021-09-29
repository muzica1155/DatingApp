using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces //idea of this repository system is we can only provide the metods that we can support for different entities now 
//its an interface then we just provide the signature of the methods that we r going to suppport in this & we need implementinh in the implementaions system
//CLASS 
{
    public interface IUserRepository
    {
        void Update(AppUser user);// update the tracking status in the entity framework to say smthing has canged 

        // Task<bool> SaveAllAsync();//change of Unitofwork
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);//get user that return app user

        Task<AppUser> GetUserByUsernameAsync(string username);//get a user by username

        // Task<IEnumerable<MemberDto>> GetMembersAsync();//in pagination // we r going to change the return type for what we r doing  with our  GetMembers
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
        //instead of returning & innumerable other memebers what we r going to do now return a page list our new class that we r created inside our helpre folder 
        Task<MemberDto> GetMemberAsync(string username);//create a corresponding methods thats going to return a memberDTo
        //so instead of returning appusers from this///<MemberDto>>// we going to be returning our memberDto & we will 
          
          //optimization query
        Task<string> GetUserGender(string username);//add new method inside & create a method specifically just to get users gender
         //optimization query
    }
}