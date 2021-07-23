using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
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
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

        }

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>///start of with identifying what claims what we put inside the token 
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)// adding our claims 
            };
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