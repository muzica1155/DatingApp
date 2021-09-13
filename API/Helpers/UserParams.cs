namespace API.Helpers
{
    public class UserParams : PaginationParams
    {
        // private const int MaxPageSize = 50;

        // public int PageNumber { get; set; } = 1; private int _pageSize = 10; public int PageSize { get => _pageSize; set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        //setuo a default gender to return which is going to be opposite of the currently logged in users gender 
        //excluding this particular use as well from what we return 
   
   // : PaginationParams
        //now we can derive from the pagination params & now our users r back to how they were before but we can use this for our likes program as well
        //
        public string CurrentUsername { get; set; }
        public string Gender { get; set; }

        //user get right to select by age 
        public int MinAge { get; set; } = 18; // give this a default value //we need to wear any alowing over 18 yr olds to access our site
        //we'll set this to initial value of 18 & we'll set another property for the max age & we'll set 
        public int MaxAge { get; set; } = 100;//this is our deafult age & this is obviously effectively going to give us all of our users & then they can go & define what age range they're looking for 
        //those 2 additional properties in place we'll go to our user repository & we'll 
    
      // going to add scorting property
       public string OrderBy { get; set; } = "LastActive";

    }
}