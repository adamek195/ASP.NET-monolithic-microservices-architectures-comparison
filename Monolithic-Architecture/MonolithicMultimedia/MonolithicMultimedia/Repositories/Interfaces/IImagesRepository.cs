using MonolithicMultimedia.Entities;
using System.IO;
using System.Threading.Tasks;

namespace MonolithicMultimedia.Repositories.Interfaces
{
    public interface IImagesRepository
    {
        public Task<Image> AddImage(Image image);
    }
}
