using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext( DbContextOptions options) : base(options)
        {

        }
        public DbSet<AppUser> User { get; set; }
        // public object Users { get; internal set; }

        //we didn't add except for the photos bcoz we didn't really need to do anything with them need to do smthing with our likess 
        public DbSet<UserLike> Likes { get; set; } // we also need to do inside here is give the entities some configuration
        // the way we do that we need to override a method inside the DB context & WE achieve this we will say protected 
        protected override void OnModelCreating(ModelBuilder builder)// we say void bcoz we dont return anthing from this & then we call it 
        {
            base.OnModelCreating(builder);// bcoz we r overriding this method just pass into the class we r driving from & we get access to that using base 
        //if we do this we can smtimes get errors when we try & add immigration 

        //we'll work on our user like entity here & we'll say builder & then we say entity <type parameter> what we wan to configure 
            builder.Entity<UserLike>()
            .HasKey(k => new {
                k.SourceUserId, k.LikedUserId // this is going to form the primary key for this particular table
            });// we'll specify that this has a key bcoz we didn't identify a primary key for this particular entity 
            //then we r going to confiure this key ourselves & going it going to a combination of the source user & liked user ID 
            // we'll achieve that will say 

            builder.Entity<UserLike>()// then we  need to do is configure the relationship inside here so what i would say isbuilder 
              .HasOne(s => s.SourceUser)// what we do here we specify the realtionship & we'll say that has 1 
              .WithMany(l => l.LikedUsers) //So a source user can like many other users is what we r saying here that we r going to specify
              .HasForeignKey(s => s.SourceUserId) // now we specify foreign key say this goes toa sourde user ID & then we'll configure the on delete behavior 
              .OnDelete(DeleteBehavior.Cascade);//then ew configure the on delete behavior// so if we delete a user we delete the realted entities
              //.OnDelete(DeleteBehavior.Cascade);// when u r using NO SQL SERVER
              //& what we'll do bcoz the other 1 is similar im just going to copy this & we r going to be super careful about what we update insdie here 
        
             builder.Entity<UserLike>() // this is the other sode of the relationship so instead of sourceUser this is going to be the LikedUser
              .HasOne(s => s.LikedUser)// A likedUser can have many liked by users & this is where the naming gets slightly tricky 
              //
              .WithMany(l => l.LikedByUsers)
              .HasForeignKey(s => s.LikedUserId) 
              .OnDelete(DeleteBehavior.Cascade); // double check & make sure its right bcoz if u start mixing up these diferent properties then it;s going tocause an issues when it comes to our queries 
              //& it might cause an issue with immigration as well 
        }
    }
}