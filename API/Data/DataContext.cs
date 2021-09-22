using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
     IdentityRoleClaim<int>, IdentityUserToken<int>>  ///instead  of the Dbcontext we r going to use identity Dbcontext // we dont hav this included //IdentityDbContext// included with our project we need to go and install 
    //bcoz it's an entity framework package then we're going to need to actually add this to it get access 
    // bcoz we want to access we want ot acces to user roles & we've given our entities a diffent key we r going to using it's instead of strings then go ahead & provide type parameters for this as well 
// if we weren;t interrested in dealing with roles & getting a list of roles this would be all we need to do However bcoz we want to get a list of the user roles then we need to go a bit further 7 we need to identify every single type unfortunately that we need to add to identity
//We cant just specify our other class which isour app user roles If we specify that then we have to specify many others 
// just add the types that we need  in here // If we specify this 1 // AppUserRole,// then we need to identity all of te different types But it does give us an opportunity to ensure they r usng integers just like we r using for the other IDs inside here 
// go down & configure ur relationship between our app user to our app role & the many to many table to join a table that we crated as well 
//add some configuration in here for that 

    {
        public DataContext( DbContextOptions options) : base(options)
        {

        }
        // public DbSet<AppUser> Users { get; set; }//we r attempting to override it the identity of context provides us with the table we need
//to populate our database with identity so we dont need to provide this Dbset just remove this 

        // public object Users { get; internal set; }

        //we didn't add except for the photos bcoz we didn't really need to do anything with them need to do smthing with our likess 
        public DbSet<UserLike> Likes { get; set; } // we also need to do inside here is give the entities some configuration
        // the way we do that we need to override a method inside the DB context & WE achieve this we will say protected 

        public DbSet<Message> Messages { get; set; }//add a DBset for the messages 
        protected override void OnModelCreating(ModelBuilder builder)// we say void bcoz we dont return anthing from this & then we call it 
        {
            base.OnModelCreating(builder);// bcoz we r overriding this method just pass into the class we r driving from & we get access to that using base 
        //if we do this we can smtimes get errors when we try & add immigration 

        builder.Entity<AppUser>()//canges after indentity  add some configuration in here for that 
           .HasMany(ur => ur.UserRole)
           .WithOne(u => u.User)
           .HasForeignKey(ur => ur.UserId)
           .IsRequired();// we just need to configure other side of this relationship as well

            builder.Entity<AppRole>()//canges after indentity  add some configuration in here for that 
           .HasMany(ur => ur.UserRole)
           .WithOne(u => u.Role)
           .HasForeignKey(ur => ur.RoleId)
           .IsRequired();
           //this is what we need in place forour data context now we inherit from identity context & give it all of those types we remove the Db set for user bcoz that's peovided by identity context 
           //& then we can configure the realtionship between the appUser & our user roles as well as the realtionship between the AppRole & our userRoles 


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

            builder.Entity<Message>()//then in our configuration for messages we wont give it a made up key like we did with the likes we to let the database 
              .HasOne(u => u.Recipient)//generate this 
              .WithMany(m => m.MessagesReceived)
              .OnDelete(DeleteBehavior.Restrict);//bcoz we dont want to remove the message id the other party hasn't deleted them themselves

              //we'll do then is do the other side of this relationship 

            builder.Entity<Message>()
              .HasOne(u => u.Sender)//this 1 is for the sender & Sender has many messahes sent & again we keep the delete behavior to restrict
              //here 
              .WithMany(m => m.MessagesSent)
              .OnDelete(DeleteBehavior.Restrict);
              // now we got our entity configured now& add migration
              
        }
    }
}