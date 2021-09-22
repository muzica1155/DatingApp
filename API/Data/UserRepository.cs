using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
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
            return await _context.Users
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

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        // & in fact taliking of lists this needs to be a page list that we r returning her now bcoz //Task<PagedList<MemberDto>>//
        // bcoz this is smthing that we r only ever going to read from wer not going to do anyhting with thses entites what we could do specify as no tracking
        {
            // return await _context.User//this r the user where we r getting our memebers this is what we want to page mean we add thingds like we take 
            // .Take(5)//we use take we specify how many users do we want to take & might wanna skip
            // .Skip(4)//skip 1st five for instance we r not going to do it inside each individual method like this bcoz this would be inefficient 
            //& would have to do the same for each different repository that we had & everytime we wanted to return a list we want smthing a bir more reusable than doing it directly
            //inside this method we do start of in our helpers folder 
            var query =  _context.Users.AsQueryable();// pagination// if we look at the type that the query is at this stage before we r done anything else then our
            //query is a type of query & this is an expression tree that's going to go to our database or entity framework is going to build up this query
            //as an expression tree & then when we execute the 2 list i think that's when it goes & executes the request in our database 
           
            // .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)//Now this is IQueryable what we can also specify inside here to be even more efficient is that by default wehn we go & get entities
            // //from entity framework entity framework applies tracking to these netities now bcoz this is going to be a list that we only ever read from
            
            // // .ToListAsync();//execute to list  which execute the databse query & of course we need to make this an . now we got our get member's methods
            // .AsNoTracking()//entites what we could do specify as no tracking & this turns off the tracking & entity framework alll 
            // // we need to do is read tihis we dont need to do anything else 

            // .AsQueryable(); // want to build onto our query still wanna select the property & with automappers taking care of that we still want us 
            //give us an opportunity to do smthing with this query & decide what we wna to filter by for instance example:- wan to return all of the users except the curretnlt logged in user
            
            query = query.Where(u => u.UserName != userParams.CurrentUsername);
            //what we see not gonna work bcoz at hte monent query projecting too & we dont want to do that at this stage we dont want to be working with the member DTO bcoz we want to filter Or buuild our expression before land & we got Our uSername, a differnt case 
            //e(u => u.Username// this is coming from our member details & this query is taking place against our users table & our user name has a dfiffernt property that 
            //Change strategy inside here 
          //got an error in Username after chnage the code//(u => u.Username//we see that our username invalid bcoz we need to target the app usernaem here 
             
           query = query.Where(u => u.Gender == userParams.Gender);//expression tree

           var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1); //just add some additional parameters inside herer to filter by the age of the user set couple of variables 
             //this is based on todays dates then what we'll do is //(-userParams.MaxAge - 1) //so that we give the accute year 
    // want to say how far do we wnat to go back ? bcoz the date of birth is a date//(-userParams.MaxAge - 1);//we inus the nO of years in a max age & in this case it's 30
    //so minus that number of years from today & alos minus 1 bcoz it it's today then they haven't had a birthday yet & we're not going by times here so it may be off by a day here & there 
    // keep thinga simple we r not going to worry about that level of accuracy for an age filter say is that the minimum date of birth let say minium age is 20 years ago
    //(-userParams.MinAge);// so we minus that from todays date with year that going to give us the maximum date of birth we're loooking at & if it's 20 from now this would mean any1 born in the year 2000 or below 
    //that y we r usig <= to there 
           var maxDob = DateTime.Today.AddYears(-userParams.MinAge); //maximum date of birth 

           query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);//wanna check >=minDob 
//EXAMPLE :- A USER WAS LOOKING FOR SOMEONE BETWEEN 20 & 30 then the max age here  would be 30 & the medium age here would be 20 
 /// check to seee if we have a userParam no need to check bcoz we r giving this a default property for that we use switch statement
           query = userParams.OrderBy switch 
           { // instead of creating new switch & then u create ur differnt cases this is a case for created this is the case 
               "created" => query.OrderByDescending(u => u.Created),  // inside here give a scenario
               // then we got 2 cases here next one is going to be what would be the default case ? 
               _ => query.OrderByDescending(u => u.LastActive)// this is the case for default:// we specify default in this switch statement is an underscore & then another area 
               // & we dont need to add breakes or anything like that 
               //need to do for sorting in the API// we just specify order by properties for whatever property it is that we do want to order By

           };
           {// use switch expression had available since c# 8 version

           }
           

            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>//want to do is project before we send it to our lists i'm just going to leave tis stuff down 
            //query.ProjectTo<MemberDto>// we r still sneding our query into a member here 
            (_mapper.ConfigurationProvider).AsNoTracking(), // we still not executing anything inside this method bcoz that still being taken care of inside our creates async method

             userParams.PageNumber, userParams.PageSize);///then we specify what type of page list we want is a page list of our member states
            //bcoz we  crated a static method on our page list called createAsync this gives us the facility to create a page list at htis stage in our repository
            //& alist of our source code variable is going to be our query pass our query into specify theuser pparameter dot page 
            //userParams.PageNumber, userParams.PageSize);// this is what we return fro our repository method 
             
             //.CreateAsync(//we r no longer executing this we r simplay passing this to another method that's gonna to execute the two list async 
            //so we r gett the page list then we create this page list & passing those pareameters
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)// thi particular method 
        {//in order ro return work with we need to get our user from our database & we need to include the photo collection And then we pass it back to our users control & if we take a look at our users control well,
            return await _context.Users
            .Include(p => p.Photos)// going to give us circular reference prob bcoz in netities in appdata //method include photo witn this request as well then //action filters 
            .SingleOrDefaultAsync(x => x.UserName == username);//getting the user by username at the moment we have access to use in a token user's username now a better method to use to do this 
       // another method would actually be the one above the one that says get user by ID async bcoz in this one we r using //getUserByIdAsync// the find async mehtod & this 1 finds an entity with the given primary key values //FindAsync(id);//
       // And bcoz our Ids automatically indexed then this 1 is going to be more efficient than the one below //GetUserByIdAsync //& also we dont include the phots in this 1 & we dont need to include thme bcoz we r not using our photos in setting the lst active property 
        }
        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
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