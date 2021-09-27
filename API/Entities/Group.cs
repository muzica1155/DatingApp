using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Group
    {
        public Group() // create a constructor just like Connection// default constructor for entity framework when it creates the tables
        //it needs an empty constructor for this then head over to IMessageRepository
        {
        }

        public Group(string name)// just allow ourselves to initialize this with the name of the group
        {
            Name = name;
            
        }

        //now this is the any property that we'll have as our key this is also to be primary key the group name will act as a primary key
        //we dont want duplicate groups in our database for we use key attributes on this & we'll use sytme component model data 
        [Key]//& this ensures the name property is the name of our group It's also index lists as well so that makes it easier for the entity framework to find this particular entity
        //
        //
        public string Name { get; set; }
        public ICollection<Connection> Connections { get; set; } = new List<Connection>();//we initialize a nwe list inside here of connection 
        // reason for tis when we create a new group we automatically want a new list inside there so we can just add the connection 
        //what will also need to do is crate this particular connection class 
    }
}