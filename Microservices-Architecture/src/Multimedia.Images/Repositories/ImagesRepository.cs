using Multimedia.Images.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Multimedia.Images.Data;
using Multimedia.Images.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Multimedia.Images.Repositories
{
    public class ImagesRepository : IImagesRepository
    {
        private readonly ImagesMultimediaContext _context;

        public ImagesRepository(ImagesMultimediaContext context)
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

        public async Task UpdateImage(int id, Image image)
        {
            var imageToUpdate = await _context.Images.SingleOrDefaultAsync(x => x.Id == id && x.UserId == image.UserId);

            if (imageToUpdate != null)
            {
                imageToUpdate.Title = image.Title;
                imageToUpdate.Description = image.Description;
                imageToUpdate.Hashtag = image.Hashtag;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteImage(int id, Guid userId)
        {
            var imageToDelete = await _context.Images.SingleOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (imageToDelete != null)
                _context.Images.Remove(imageToDelete);

            await _context.SaveChangesAsync();
        }
    }
}
