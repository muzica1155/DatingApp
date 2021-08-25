//after inputing APiname , apiKey & apiSecret we headback over here configuration we r actually going to strongly type it 
//to store our cloadinary setting inside here means set up properties for these different key elements 
namespace API.Helpers
{
    public class CloudinarySettings
    {
        public string CloudName { get; set; }
        public string ApiKey { get; set; }

        public string ApiSecret { get; set; }
        
    }
}