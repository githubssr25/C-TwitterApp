using AutoMapper;
using MyBackendApp.DTOs;
using MyBackendApp.Entities;

namespace MyBackendApp.Mappers
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Credentials.Username));

            CreateMap<UserRequestDto, User>()
                .ForMember(dest => dest.Credentials, opt => opt.MapFrom(src => src.Credentials))
                .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.Profile))
                .ForMember(dest => dest.Joined, opt => opt.Ignore());

            // Profile mappings
            CreateMap<ProfileDto, Entities.Profile>().ReverseMap(); // Fully qualify Profile

            // Credentials mappings
            CreateMap<Credentials, CredentialsDto>().ReverseMap();

            // Tweet mappings
            CreateMap<Tweet, TweetResponseDto>();
        }
    }
}


// using AutoMapper;
// using MyBackendApp.DTOs;
// using MyBackendApp.Entities;

// namespace MyBackendApp.Mappers
// {
//     public class ProfileMapper : Profile
//     {
//         public ProfileMapper()
//         {
//             CreateMap<Profile, ProfileDto>().ReverseMap();
//         }
//     }
// }
// THEY SAID ITS REDUNDANT TO HAVE THIS dont need it was initially profileMapper.cs 