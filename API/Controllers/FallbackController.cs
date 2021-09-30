//when we go to localhost 5001 it knows what to do by dafault it gonna fish out the index page indexed to each HTML 
//But if we go anywhere else it's got no idea about what to do wit this particular route 
//wenne dto add is msting refered to as a fallback controller what to do if it cannot find a route 
//now all of our routes begins with API & all of our controllers if we go to our users controller  all of our controllers know to do with this 
//route In fact we added it to the base API controller so our API server that knows what to do with any routes beginning with API & then 
//the controller is happy with them But for members it hasn't got a clue we dont register any of our angular routes inside here
// what we do create a controller from here we r not gonna derive from our base API controller we gonna derive from controller here
//differnt between this & what we r using is this is a clas for an MVC controller with views Support now r angular app is the view for our app
//7 what we gonna do is tell this full back controller what files to serve & then we r gonna to tell our API what to do any routes that 
//it doesn't understand & the idea is that we send them to the indexed or HTML bcoz that's were our angular app is being served from & our angular application
//knows what to do with all of these routes a
//what we need to do inside this particular controller will just create an action result 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FallbackController: Controller
    {
        public ActionResult Index()
        {
             //inside we r gonna return physical file the index to it html & the idea of this is that id our API doesn't know what to do with the roots
             //then it falls back to this controller & it returns this particular file now our index e-mail is responsible for loading our angular app & our angular app definitely knows
             //what to do with the roots for the angular app a
             return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), 
             "wwwroot", "index.html"), "text/HTML");
             //"index.html")// hwich gonna be file //"wwwroot",//specify the folder//"text/HTML"//file type///////////////
             // waht to do now tell our start up class what to do where to fall back to & wanna be fallback to this controller
             //
                     }
    }
}