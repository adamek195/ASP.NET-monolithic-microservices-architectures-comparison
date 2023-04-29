using Microsoft.EntityFrameworkCore;
using MonolithicMultimedia.Data;
using MonolithicMultimedia.Entities;
using MonolithicMultimedia.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Image> GetImage(int id)
        {
            var image = await _context.Images.SingleOrDefaultAsync(x => x.Id == id);

            return image;
        }

        public async Task<Image> GetUserImage(int id, Guid userId)
        {
            var image = await _context.Images.SingleOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            return image;
        }

        public async Task<List<Image>> GetImages()
        {
            var images = await _context.Images.OrderByDescending(x => x.CreationDate).ToListAsync();

            return images;
        }

        public async Task<List<Image>> GetImagesByHashtag(string hashtag)
        {
            var images = await _context.Images.Where(x => x.Hashtag.Contains(hashtag)).OrderByDescending(x => x.CreationDate).ToListAsync();

            return images;
        }

        public async Task<List<Image>> GetUserImages(Guid userId)
        {
            var images = await _context.Images.Where(x => x.UserId == userId).OrderByDescending(x => x.CreationDate).ToListAsync();

            return images;
        }

        public async Task<Image> AddImage(Image image)
        {
            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return image;
        }

        public async Task DeleteImage(int imageId, Guid userId)
        {
            var imageToDelete = await _context.Images.SingleOrDefaultAsync(x => x.Id == imageId && x.UserId == userId);

            if (imageToDelete != null)
                _context.Images.Remove(imageToDelete);

            await _context.SaveChangesAsync();
        }
    }
}
