namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }//DTO Data transfer Object
        //when we return appuser properties to a appuer to a client user 
        //another reason to use DTO hindinf certain porperties that maps directly to our database 
        // we can flaten object or we got nested object inside our code and also we lokk relationship we could have circular references between an entity to another cause circuler reference excuptions 
        //main use of dto to our acount controller so we can receive the properties inside the object 

        public byte[] PasswordHash { get; set; } //byte[] arays 

        public byte[] PasswordSalt { get; set; }

        
    }
}