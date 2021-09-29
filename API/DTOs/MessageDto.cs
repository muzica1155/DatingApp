
using System;
using System.Text.Json.Serialization;

namespace API.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }

        public int SenderId { get; set; }//we r also gonna do is track the sender Id 
        public string SenderUsername { get; set; }
        public string SenderPhotoUrl { get; set; }

        public int RecipientId { get; set; }

        public string RecipientUsername { get; set; }

        public string RecipientPhotoUrl { get; set; } //So that we can display the image of the user that sent the message

         public string Content { get; set; }// then we have some messages to specific properties so we'll have a string for the contents

         public DateTime? DateRead { get; set; }/// we r gonna make this optional bcoz we want this to be null if the message has not been read 
         //& what we'l also do is have a date time 
         
         public DateTime MessageSent { get; set; } 
         // this is going to make a our message DTO & what we'l do we'll get bac to out message repository 

         //after fixing te error from IMessage Repository later on ahead back to data folder create the implementation class for our repository
         // so we'll say message repository

//optimization query
         [JsonIgnore]
         public bool SenderDeleted { get; set; }//add properties for 

          [JsonIgnore] // then these r not gonna send back to the client but we will have access to them inside our repository after we've 
          //projected to a message Dto
         public bool RecipientDeleted { get; set; }// now we dont want to send this properties back without Dto & as long as these properties match 
         //the names inside our actual message entity then all the map is gonna map these for us directly anyway what we can so specify 
         //optimization query
        
    }
}