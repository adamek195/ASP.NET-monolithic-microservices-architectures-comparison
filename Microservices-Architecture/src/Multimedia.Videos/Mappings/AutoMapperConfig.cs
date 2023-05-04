using AutoMapper;
using Multimedia.Videos.Dtos;
using Multimedia.Videos.Entities;

namespace Multimedia.Videos.Mappings
{
    public class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CommandVideoDto, Video>();
                cfg.CreateMap<VideoDto, Video>();
                cfg.CreateMap<Video, VideoDto>();
            })
            .CreateMapper();
    }
}
