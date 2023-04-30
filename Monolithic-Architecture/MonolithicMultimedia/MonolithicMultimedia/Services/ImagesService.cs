using AutoMapper;
using MonolithicMultimedia.Dtos;
using MonolithicMultimedia.Entities;
using MonolithicMultimedia.Exceptions;
using MonolithicMultimedia.Repositories.Interfaces;
using MonolithicMultimedia.Services.Interfaces;
using MonolithicMultimedia.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MonolithicMultimedia.Services
{
    public class ImagesService : IImagesService
    {
        private readonly IImagesRepository _imagesRepository;
        private readonly MediaRepositorySettings _mediaRepositorySettings;
        private readonly IMapper _mapper;

        public ImagesService(IImagesRepository imagesRepository, MediaRepositorySettings mediaRepositorySettings, IMapper mapper)
        {
            _imagesRepository = imagesRepository;
            _mediaRepositorySettings = mediaRepositorySettings;
            _mapper = mapper;
        }

        public async Task<ImageDto> GetImage(int id)
        {
            var image = await _imagesRepository.GetImage(id);

            if (image == null)
                throw new NotFoundException("Image with this id does not exist.");

            return _mapper.Map<ImageDto>(image);
        }

        public async Task<List<ImageDto>> GetImages()
        {
            var images = await _imagesRepository.GetImages();

            return _mapper.Map<List<ImageDto>>(images);
        }

        public async Task<List<ImageDto>> GetImagesByHashtag(string hashtag)
        {
            var images = await _imagesRepository.GetImagesByHashtag(hashtag);

            return _mapper.Map<List<ImageDto>>(images);
        }

        public async Task<List<ImageDto>> GetUserImages(string userId)
        {
            var images = await _imagesRepository.GetUserImages(Guid.Parse(userId));

            return _mapper.Map<List<ImageDto>>(images);
        }

        public async Task CreateImage(CommandImageDto commandImageDto, Stream stream, string userId, string fileName)
        {
            var imageToAdd = _mapper.Map<Image>(commandImageDto);
            imageToAdd.CreationDate = DateTime.Now;
            imageToAdd.UserId = Guid.Parse(userId);

            var resourcePath = _mediaRepositorySettings.ImagePath + '\\' + userId + '\\' + fileName;
            imageToAdd.Path = resourcePath;
            var directoryPath = Path.GetDirectoryName(resourcePath);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            using (var fs = File.Create(resourcePath))
                stream.CopyTo(fs);

            await _imagesRepository.AddImage(imageToAdd);
        }

        public async Task UpdateImage(int id, string userId, CommandImageDto commandImageDto)
        {
            var imageToUpdate = await _imagesRepository.GetUserImage(id, Guid.Parse(userId));

            if (imageToUpdate == null)
                throw new NotFoundException("Image with this id does not exist.");

            imageToUpdate = _mapper.Map<Image>(commandImageDto);
            imageToUpdate.UserId = Guid.Parse(userId);

            await _imagesRepository.UpdateImage(id, imageToUpdate);
        }

        public async Task DeleteImage(int id, string userId)
        {
            var imageToDelete = await _imagesRepository.GetUserImage(id, Guid.Parse(userId));

            if (imageToDelete == null)
                throw new NotFoundException("Image with this id does not exist.");

            await _imagesRepository.DeleteImage(id, Guid.Parse(userId));
        }
    }
}
