using MonolithicMultimedia.Dtos;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MonolithicMultimedia.Services.Interfaces
{
    public interface IVideosService
    {
        Task<VideoDto> GetVideo(int id);

        Task<List<VideoDto>> GetVideos();

        Task<List<VideoDto>> GetVideosByHashtag(string hashtag);

        Task<List<VideoDto>> GetUserVideos(string userId);

        Task<List<VideoDto>> GetVideosByEmail(string email);

        Task CreateVideo(CommandVideoDto commandVideoDto, Stream stream, string userId, string fileName);

        Task UpdateVideo(int id, string userId, CommandVideoDto commandVideoDto);

        Task DeleteVideo(int id, string userId);
    }
}
