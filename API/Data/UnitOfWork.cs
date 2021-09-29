using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork //what we r doing to be doing from our units of work is creating instance 
    //of the repositories & we r gonna pass in what this would have in its constructor so if we take a look at
    //the user repository as an example Then to use a repository as a constructor & at the moment we r using 
    //dependency injection to get this but what we r gonna to do is create these new instance of the repositories
    //via our units of work So instead of using dependency injection injection here we r gonna to remove these 
    //repositories from that & we r gonna be using the units of work to create these instance 
    //what we do instead of throw new NotImplementedException(); ...
    {

        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IUserRepository UserRepository => new UserRepository(_context, _mapper);
        // what we ahve to pass this is  an instance of the data context & the map for that genrate a contractor after this we nned to pass
        //the user repository the context & the mapper SO the same for messages repository

        public IMessageRepository MessageRepository => new MessageRepository(_context, _mapper);

        public ILikesRepository LikesRepository => new LikesRepository(_context);

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;//implement our complete method this is just gonna be what we were doing inside our repositories so all of the chanhes that entity 
            //framework has tracked no matter which repository we used to do smthing then we r gonna be using this 1 to save al of our changes to
        //& just make sure that we've had some changes saved & we'll make this async 
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();//just gonna see if it has changes
            //so anything entity framework is tracking if it has smthing then that will return true after this go to application services extension
        }
    }
}