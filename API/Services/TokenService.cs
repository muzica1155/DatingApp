using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;//symmetric encryption, by the way, is a type encryption where only one key is used to both encryption and decrypt electronic information. 
        //same key is used to both side JWT token & make sure the signature is verified the other type of course, is a symmetric encryption where 
        // a pair of keys, one public, one private is used to encrypt & decrypt messages & thats hows https and SSL works asymmetrically 
        //whereas this JSon what token is going to use a symmetric key?
        //because this key doesn't need to leave the server it can remain on the server & does,'t need to go anywhere & learn 

        // public TokenService(IConfiguration config)
        //& noww add our userManager inside here change after identity 
        private readonly UserManager<AppUser> _userManager;

        public TokenService(IConfiguration config, UserManager<AppUser> userManager)//bocz thisis how we get the users rolls & will pass in the appUSer & we call it user 
        {
            _userManager = userManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

        }

//changes after identity 
        // public string CreateToken(AppUser user)// convert this into task just to convert this method into async 
        
        public async Task<string> CreateToken(AppUser user)//we need to update the interface as well so that we also specify that we r returning
    //a task from that no we have to update the itoken service 
        {
            var claims = new List<Claim>///start of with identifying what claims what we put inside the token 
            {
                // new Claim(JwtRegisteredClaimNames.NameId, user.UserName)// adding our new claims we r settig the name ID to the user's UserName//but we r going to change during action filters 
            //we can useit either or really butthe name Id is the only ID claim name tha twe really have acces to //Options we have then there's no specify Id field we do have a nameID & we also got a uniqueName property 
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),// after change now we have to go to our claims principle extension 7 update that bcoz we r going to need to change that inside 
            
            //changes after identity description
            //we already got 2 claims going back already for our userID & our username & what we also want to pass inside this token is the rolls that user belongs to 
            //now ourtoken is a safe place to put the rolls the user canot modify their rolls to trick ourselves into thinking that they r an admin it will not work wants to take & has been modified 
    //Unless u happen to know the secret key that u r using on a server then u cannot modify the roll & pretend that u r got an elevated level of permissions 
//So the token is a safe place & we trust out token on the server when a user presents it 

            // we use the unique name for the username & we'll use the nameID for the users ID whic is going to be the end 
            //set userID & bcoz the Id is an integer & claims have to be string then we r also going to set this using the two string property 
            };
            // changes after identity 

            var roles = await _userManager.GetRolesAsync(user);//then we go & get///inside outr roles is gonna be list ofroles that this user belongs to 
            // we want to add them into our list of claims

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));//passin the role that we've got from our select so we slect the roles from a list of roles
            //& then we crate a new claims &rather than using JwtRegisteredClaimNames  we r gonna be using clean types bcoz 
        //JwtRegisteredClaimNames do not have an option for a role inside here so whilst these r the JWT register claims names ther's nothing to stop us using custom 1s & 
        //inside the claim Types we have here is a roll claims & we can use that to add it to our token //Now we need to update AccountController

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),// creating some credentials  //here we specify what goes inside are taken so first of all we need the subject & this is going to cpntain our claims identify & we all pass in the claims
                Expires = DateTime.Now.AddDays(7), //token for how much is this valid for 
                SigningCredentials = creds // this is our token 
            };// describing how token going to look 
            //now we have to create a token handler for
            var tokenHandler = new JwtSecurityTokenHandler();// we need this token handler smthinf need to create the token & the return
            var token = tokenHandler.CreateToken(tokenDescriptor); //in parameters // a token descriptor has passed & created a token
            return tokenHandler.WriteToken(token); //return the written token to whoever needs it
        }
    }
}