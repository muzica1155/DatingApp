// likes params bcoz we wna to end pagination 7 will inherit from paginationparams 
namespace API.Helpers
{
    public class LikesParams : PaginationParams
    { //
       //we need likes controller // take a look what we need & what we r taking form the parameters ? so we r getting the predicate //(string predicate)
       //From the query string & we also need the user ID as well //User.GetUserId());// what we do is we'll add thses 2 properties inside likeparam
        public int UserId { get; set; }
        public string Predicate { get; set; }// go ahead ILikesRepository
    }
}