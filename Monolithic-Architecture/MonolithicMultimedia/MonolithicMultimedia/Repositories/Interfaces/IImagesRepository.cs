using MonolithicMultimedia.Entities;
using System.Collections.Generic;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MonolithicMultimedia.Repositories.Interfaces
{
    public interface IImagesRepository
    {
        Task<Image> GetImage(int id);

        Task<Image> GetUserImage(int id, Guid userId);

        Task<List<Image>> GetImages();

        Task<List<Image>> GetImagesByHashtag(string hashtag);

        Task<List<Image>> GetUserImages(Guid userId);

        public Task AddImage(Image image);

        public Task UpdateImage(int id, Image image);

        public Task DeleteImage(int id, Guid userId);
    }
}
