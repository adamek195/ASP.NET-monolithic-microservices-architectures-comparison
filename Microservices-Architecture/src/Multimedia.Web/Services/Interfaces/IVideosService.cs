using Multimedia.Web.Dtos;
using System.IO;
using System.Threading.Tasks;

namespace Multimedia.Web.Services.Interfaces
{
    public interface IVideosService
    {
        Task<T> GetVideo<T>(int id, string token = null);

        Task<T> GetVideos<T>(string token = null);

        Task<T> GetVideosByHashtag<T>(HashtagDto hashtagDto, string token = null);

        Task<T> GetUserVideos<T>(UserIdDto userIdDto, string token = null);

        Task CreateVideo(CommandVideoDto commandVideoDto, Stream stream, string fileName, string token = null);

        Task<T> UpdateVideo<T>(int id, CommandVideoDto commandVideoDto, string token = null);

        Task<T> DeleteVideo<T>(int id, UserIdDto userIdDto, string token = null);
    }
}
