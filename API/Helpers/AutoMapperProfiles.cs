using System.Linq;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser,MemberDto>()
            .ForMember(dest => dest.MainPhotoUrl, options => 
            options.MapFrom(src => src.Photos.FirstOrDefault(photo => photo.IsMain == true).Url))
            .ForMember(dest => dest.Age, opt => 
            opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo,PhotoDto>();
            CreateMap<UpdateMemberDto,AppUser>();
            CreateMap<UserRegisterDto,AppUser>();
        }
    }
}