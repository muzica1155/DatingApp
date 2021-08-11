using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{//this is going to use or implement i use a repository interface 
    public class UserRepository : IUserRepository//UserRepository :// accessing our Db context we r going to need a constructor
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<MemberDto> GetMemberAsync(string username)//talk about projection what if we dont wan to use automapper
        {
            return await _context.User
            .Where(x => x.UserName == username)// use consitions we want to get the user by username 
            // .Select(user => new MemberDto //then we have to do this manually mapping all the properties 
            // //in this select statement we start manually mapping the properties need to select from our database put inside & return for our memberDTO
            // { Id = user.Id, 
            //     Username = user.UserName//we got 20 properties in here no need to go to through the whole thing bcoz of Auto mapper helps us out here 
            // }) //get to rid of this error we need to execurte th request //this goes to our database
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider) //this an auto mapper & we need to bring this in form auto mapper but need to add parameters
            //it wants configurations provider is to do provide this pevar mapper configurations we to inject IMapper in public UserRepository 
            //(_mapper.ConfigurationProvider)//we can go & get the configurations that we provided in our alternative profiles here 
            //.ProjectTo<MemberDto>(_mapper.ConfigurationProvider)// selecting the properties we want directly from the database 
            .SingleOrDefaultAsync();
        }//.SingleOrDefaultAsync();//this where we execute the query 

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await _context.User
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .ToListAsync();//execute to list  which execute the databse query & of course we need to make this an . now we got our get member's methods
            
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.User.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {//in order ro return work with we need to get our user from our database & we need to include the photo collection And then we pass it back to our users control & if we take a look at our users control well,
            return await _context.User
            .Include(p => p.Photos)// going to give us circular reference prob bcoz in netities in appdata 
            .SingleOrDefaultAsync(x => x.UserName == username);
        }
        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.User
            .Include(p => p.Photos)
            .ToListAsync();///method to get and go all of our users 
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;//returning a boolean to say that if our changes have been saved
        }//return a boolean from this make sure that greater than zero changes have been saved into our database
        /// if smthing changed or saved then it going to return a value greater than zero bcoz the SaveChangesAsync()// return an integer from this particilar method for a number of changes saved in database
        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;//update our user //goona do mark this entity as it has been modified 
        }//this entity framework update and add aflag to the entity to say yes that been modifed 
    }
}