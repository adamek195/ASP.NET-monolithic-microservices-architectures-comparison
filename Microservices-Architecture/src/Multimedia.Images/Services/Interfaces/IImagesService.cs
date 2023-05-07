using Multimedia.Images.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Multimedia.Images.Services.Interfaces
{
    public interface IImagesService
    {
        Task<ImageDto> GetImage(int id);

        Task<List<ImageDto>> GetImages();

        Task<List<ImageDto>> GetImagesByHashtag(HashtagDto hashtagDto);

        Task<List<ImageDto>> GetUserImages(string userId);

        Task<ImageDto> CreateImage(CommandImageDto commandImageDto, Stream stream, string fileName);

        Task UpdateImage(int id, CommandImageDto commandImageDto);

        Task DeleteImage(int id, UserIdDto userDto);
    }
}
