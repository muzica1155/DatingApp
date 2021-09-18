// So this is for our relationship these properties define the realtionship between the app user & the message
using System;

namespace API.Entities
{
    public class Message
    {
        public int Id { get; set; }

        public int SenderId { get; set; }//we r also gonna do is track the sender Id 
        public string SenderUsername { get; set; }

        public AppUser Sender { get; set; }// related property which is going to be the appuser & we call it sender

        public int RecipientId { get; set; }

        public string RecipientUsername { get; set; }

        public AppUser Recipient { get; set; } 

         public string Content { get; set; }// then we have some messages to specific properties so we'll have a string for the contents

         public DateTime? DateRead { get; set; }/// we r gonna make this optional bcoz we want this to be null if the message has not been read 
         //& what we'l also do is have a date time 
         public DateTime MessageSent { get; set; } = DateTime.Now; ///thi si going to be date time that message is sent 
         // as soon as we create a new instance of this then we set the time to the current server timestamp 
           
         public bool SenderDeleted { get; set; }//then we have a couple of additional properties and we'll have a boolean

         public bool RecipientDeleted { get; set; }// & have another boolean for recipient has deleted the meassage
         // & the reason for this if a user decides to delete a message thet they have sent then we r not going to 
         //delete it from the recipient's view of the messages the only time we delete a message from the server if both the sender &
         // the recipient have both deleted the message & then what we can do is we can go accross to our app user class 

        
    }
}