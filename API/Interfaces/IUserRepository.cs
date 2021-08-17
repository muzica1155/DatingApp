using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces //idea of this repository system is we can only provide the metods that we can support for different entities now 
//its an interface then we just provide the signature of the methods that we r going to suppport in this & we need implementinh in the implementaions system
//CLASS 
{
    public interface IUserRepository
    {
        void Update(AppUser user);// update the tracking status in the entity framework to say smthing has canged 
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);//get user that return app user

        Task<AppUser> GetUserByUsernameAsync(string username);//get a user by username

        Task<IEnumerable<MemberDto>> GetMembersAsync();
        Task<MemberDto> GetMemberAsync(string username);//create a corresponding methods thats going to return a memberDTo
        //so instead of returning appusers from this///<MemberDto>>// we going to be returning our memberDto & we will 
    }
}