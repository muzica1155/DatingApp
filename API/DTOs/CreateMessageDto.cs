// we create 2 properties 
namespace API.DTOs
{
    public class CreateMessageDto
    {
        public string RecipientUsername { get; set; }

        public string Content { get; set; } // this takes care of our set up for DTO the entity the repository although we haven't implemented
        //logic yet further more we look at adding a message controller next\
        
    }
}