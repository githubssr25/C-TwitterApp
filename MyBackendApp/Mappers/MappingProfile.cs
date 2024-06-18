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
            CreateMap<UserRequestDto, User>();

            // Profile mappings
            CreateMap<MyBackendApp.Entities.Profile, ProfileDto>().ReverseMap();

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