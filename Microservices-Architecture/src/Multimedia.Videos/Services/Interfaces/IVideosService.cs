using Multimedia.Videos.Dtos;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Multimedia.Videos.Services.Interfaces
{
    public interface IVideosService
    {
        Task<VideoDto> GetVideo(int id);

        Task<List<VideoDto>> GetVideos();

        Task<List<VideoDto>> GetVideosByHashtag(HashtagDto hashtagDto);

        Task<List<VideoDto>> GetUserVideos(UserIdDto userIdDto);

        Task<VideoDto> CreateVideo(CommandVideoDto commandVideoDto, Stream stream, string fileName);

        Task UpdateVideo(int id,  CommandVideoDto commandVideoDto);

        Task DeleteVideo(int id, UserIdDto userDto);
    }
}
