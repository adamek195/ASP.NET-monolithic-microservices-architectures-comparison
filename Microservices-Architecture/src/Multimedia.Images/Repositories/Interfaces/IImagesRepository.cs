using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Multimedia.Images.Entities;

namespace Multimedia.Images.Repositories.Interfaces
{
    public interface IImagesRepository
    {
        Task<Image> GetImage(int id);

        Task<Image> GetUserImage(int id, Guid userId);

        Task<List<Image>> GetImages();

        Task<List<Image>> GetImagesByHashtag(string hashtag);

        Task<List<Image>> GetUserImages(Guid userId);

        public Task<Image> AddImage(Image image);

        public Task UpdateImage(int id, Image image);

        public Task DeleteImage(int id, Guid userId);
    }
}
