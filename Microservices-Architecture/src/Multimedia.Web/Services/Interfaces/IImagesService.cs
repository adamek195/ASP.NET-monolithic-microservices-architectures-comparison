using Multimedia.Web.Dtos;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Multimedia.Web.Services.Interfaces
{
    public interface IImagesService
    {
        Task<T> GetImage<T>(int id, string token = null);

        Task<T> GetImages<T>(string token = null);

        Task<T> GetImagesByHashtag<T>(HashtagDto hashtagDto, string token = null);

        Task<T> GetUserImages<T>(UserIdDto userIdDto, string token = null);

        Task<T> CreateImage<T>(CommandImageDto commandImageDto, Stream stream, string userId, string fileName, string token = null);

        Task<T> UpdateImage<T>(int id, CommandImageDto commandImageDto, string token = null);

        Task<T> DeleteImage<T>(int id, UserIdDto userIdDto, string token = null);
    }
}
