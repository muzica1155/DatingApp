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
        }
    }
}