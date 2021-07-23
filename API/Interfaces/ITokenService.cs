using API.Entities;

namespace API.Interfaces
{
    ///interface is contract between itself and any class that implements it and
    ///this contract states that any class that implements it this interface will implement the porperties, methods, and events.
    //interface does not contain any implementations logic and only contain the signature of the functionality the interface provide 
    //this service is gonna have a single method  
    //
    public interface ITokenService ///whenit come to interface we always prefix them with them in I
    {
        string CreateToken(AppUser user);
    }
}