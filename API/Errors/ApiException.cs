namespace API.Errors
{
    public class ApiException
    {
        public ApiException(int statusCode, string messages = null, string details = null) //take sour properties & populkats them insdide the constructor
        { //statuscode whatever happens for exception going to be 500 
        //if we dont provide a message & we dont provide details then both of these properties are just going toset null
            StatusCode = statusCode;
            Messages = messages;
            Details = details;
        }

        public int StatusCode { get; set; }  // we r going to have free properties

      public string Messages { get; set; }
      public string Details { get; set; }// details in this case is really just going to be the stack trace that we get in order to make this easier to use // easier to use we gice constractor

    }
}