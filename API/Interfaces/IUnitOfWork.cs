using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get;}// we gonna specify that we want to return our repository  so we r gonna say I use a repository & call it
        IMessageRepository MessageRepository { get;}//
        ILikesRepository LikesRepository { get;}
        Task<bool> Complete();// this is gonna to be method to save all of our changes so we r gonna to call it completes
        bool HasChanges();//add 1 more helper methopd // this 1 is for see if the entity framework has been tracking
        //or ahs any changes we;ll need to use that in 1 specific place 
    }
}