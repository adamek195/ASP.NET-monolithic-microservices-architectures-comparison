using AutoMapper;
using Multimedia.Images.Dtos;
using Multimedia.Images.Entities;

namespace Multimedia.Images.Mappings
{
    public class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CommandImageDto, Image>();
                cfg.CreateMap<ImageDto, Image>();
                cfg.CreateMap<Image, ImageDto>();
            })
            .CreateMapper();
    }
}
