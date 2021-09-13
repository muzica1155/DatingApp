//we dont have many users to return when we do this we still do add pagination to this as we saw when i made the little error
// in the service we return all of the users if we don't specify pagination so there's nothing to stop a user overriding 
//what we r done here to get everything ba
// let begin with user params where we set uo pagination for our users look at userparams we like to use this
// private const int MaxPageSize = 50;
//public int PageNumber { get; set; } = 1;
       // private int _pageSize = 10;
       // public int PageSize 
      //  { get => _pageSize;  set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
// Also for our likes entity & we do create a new class that's going to store this info & then for any other class that needs pagination 
     //it can inherit it can inherit from that class
     //       
     //   
//
namespace API.Helpers
{
    public class PaginationParams
    {
        private const int MaxPageSize = 50;

        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize 
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;

        }
        
    }
}