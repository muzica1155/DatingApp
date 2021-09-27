using System;
using System.Linq;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{ //Auto MApper is it helps us map from one object to another thats its job, maps from one object to another 
    public class AutoMapperProfiles : Profile//we need to drive in this class form profile & we can bring in using auto map 
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()///where we want to map from and where we want to 
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => //when we map an individual property we give it the destination property the photo url 
            //
            src.Photos.FirstOrDefault(x => x.IsMain).Url))//we need to do make sure that we can populate that one indicidual property so we can add some extra configuration here 
            //for member means which property that we want to affect and first parameter we pass is the destination
            //What property are we looking to affect ?//opt.MapFrom //we can tell it where we want to map from very specifically 
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));//new mapping configuration 
            CreateMap<Photo, PhotoDto>(); //a map from our photo to or form our photo entity to our photo
            CreateMap<MemberUpdateDto, AppUser>();
            //want to go from our member update DTO to our app user 
            //<MemberUpdateDto, AppUser>().ReverseMap// ther eis a option called reverse mapa we could reverse map
            //but we r not going from our member DTo we r going from a differnt DTO
            //use separate configuration option for this 
            CreateMap<RegisterDTo, AppUser>();//create a new mapping to go from [registeration form]
            // CreateMap<RegisterDTo, AppUser>();// this means we dont really need to map the properties htat we r receiving in our account a controller
            
            CreateMap<Message, MessageDto>()// we wan to go from our message to our messageDto
                 .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => 
                 src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
            //inside here er r going to need to give this some configuration bcoz there's there a couple of properties or 1 property that
            // we cannot get automaticaly to do for us & that's fr the use of it 
            // we've got to go a few more levels deep in this particular case 
            //& also need to stay the same for the recipients 
            .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src => 
                 src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
                 // this i will save us typing out all of that mapping code & what we'l also do is we're going to create a DTO for the receiving
                 //of a message as well While we r here lets setup out that we can then forcus on creatng that 

                CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc)); //create a mapping for time zone UTC now 
                //use a function in order to map a called convert using 
               // DateTime.SpecifyKind)// this gives us the opportunity to tell our datetime if it's UTC or if it's local time 
               //(d, DateTimeKind.Utc)); // what this means is tha twhen we return our dates to the client we r gonna to have that Z on the ned of it so 
               // let's go take a look & if I refresh this page 

        }
    }
}