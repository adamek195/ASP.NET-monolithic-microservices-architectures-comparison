using AutoMapper;
using Multimedia.Users.Dtos;
using Multimedia.Users.Entities;

namespace Multimedia.Users.Mappings
{
    public class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<LoginUserDto, User>()
                    .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
                cfg.CreateMap<UserDto, User>();
                cfg.CreateMap<CreateUserDto, User>()
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                    .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
            })
            .CreateMapper();
    }
}
