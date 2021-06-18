using System.Linq;
using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser,MemberDto>()
            .ForMember(dest => dest.MainPhotoUrl,options => 
            options.MapFrom(src => src.Photos.FirstOrDefault(photo => photo.IsMain == true).Url));
            CreateMap<Photo,PhotoDto>();
        }
    }
}