using AutoMapper;
using MonolithicMultimedia.Dtos;
using MonolithicMultimedia.Entities;

namespace MonolithicMultimedia.Mappings
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
                cfg.CreateMap<CommandImageDto, Image>();
            })
            .CreateMapper();
    }
}
