using MonolithicMultimedia.Data;
using MonolithicMultimedia.Entities;
using MonolithicMultimedia.Repositories.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace MonolithicMultimedia.Repositories
{
    public class ImagesRepository : IImagesRepository
    {
        private readonly MonolithicMultimediaContext _context;

        public ImagesRepository(MonolithicMultimediaContext context)
        {
            _context = context;
        }

        public async Task<Image> AddImage(Image image)
        {
            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return image;
        }
    }
}
