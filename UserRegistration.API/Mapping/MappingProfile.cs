using UserRegistration.API.Models;
using UserRegistration.Domain.Entities;
using System.Text.RegularExpressions;

namespace UserRegistration.API.Mapping
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<UserCreationDto, User>()
                .ForMember(dest => dest.CPF, opt => opt.MapFrom(src => Regex.Replace(src.Cpf, "[^0-9]", "")))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

            CreateMap<User, UserResponseDto>()
               .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.Profile));

            CreateMap<Profile, ProfileDto>();
        }
    }
}
