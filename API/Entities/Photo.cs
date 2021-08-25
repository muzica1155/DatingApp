using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    //we r not getting photo independently photo collections no necessary need a DB set need photo entity just to ensure 
    [Table("Photos")]// when entity framework creates thois table it going to call it phots 
    public class Photo
    {
        public int Id { get; set; }

        public string Url { get; set; }
        public bool IsMain { get; set; }

        public string PublicId { get; set; }//we created this string for the public ID so that we would n't need to run it now which would mean we need to add any migration already got in place 

        //if u want to enable cacate delete make sure our app use a property in that table cannot be null then we need to do called fully defining the relationship
        //and to fully define relationship tell photo entity about appuser class will addd a property fully appuse
        public AppUser AppUser { get; set; }
        //in eager loading what going to returned is for each photo its going to attempt to return an app user 
        //and our app user has a collection of photos and our photo has an app user etc this going to give us circular references to photos

        public int AppUserId { get; set; }
    }
}