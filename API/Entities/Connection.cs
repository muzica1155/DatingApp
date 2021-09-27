namespace API.Entities
{
    
    public class Connection// just  to make this class a little bit easier to use we'll just add a constructor to just we can pass in 
    //when we create a new instance of this we're just gonna to pass in a connectionID & the reason for this is that when we create a new
    //instance of our connection we just need to open parentheses & pass in these 2 properties & bcoz this is an entity what we'll do is we'll
    //give entity framework a dafault constructor as well which it needs otherwise we;ll probably see an error 

    { // by convention if we give it name & then ID entity framework is gonna automatically consider this the primary key & then will have 
        public Connection()//generate a deafault constructor with no parameters go similar thing to te group 
        {
        }

        public Connection(string connectionId, string username)
        {
            ConnectionId = connectionId;
            Username = username;
        }

        // another property for the sting of the username 
        public string ConnectionId { get; set; }//our connection take 2 properties

        public string Username { get; set; }//
    }
}