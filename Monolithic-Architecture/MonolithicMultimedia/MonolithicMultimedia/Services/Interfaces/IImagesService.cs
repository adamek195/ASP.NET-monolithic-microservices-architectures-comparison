using MonolithicMultimedia.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MonolithicMultimedia.Services.Interfaces
{
    public interface IImagesService
    {
        Task<ImageDto> GetImage(int id);

        Task<List<ImageDto>> GetImages();

        Task<List<ImageDto>> GetImagesByHashtag(string hashtag);

        Task<List<ImageDto>> GetUserImages(string userId);

        Task<List<ImageDto>> GetImagesByEmail(string email);

        Task CreateImage(CommandImageDto commandImageDto, Stream stream, string userId, string fileName);

        Task UpdateImage(int id, string userId, CommandImageDto commandImageDto);

        Task DeleteImage(int id, string userId);
    }
}
