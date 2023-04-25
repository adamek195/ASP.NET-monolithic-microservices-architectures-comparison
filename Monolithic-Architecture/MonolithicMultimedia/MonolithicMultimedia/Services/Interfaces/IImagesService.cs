using MonolithicMultimedia.Dtos;
using System.IO;
using System.Threading.Tasks;

namespace MonolithicMultimedia.Services.Interfaces
{
    public interface IImagesService
    {
        Task CreateImage(CommandImageDto commandImageDto, Stream stream, string userId, string fileName);
    }
}
