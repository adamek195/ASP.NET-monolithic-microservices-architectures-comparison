using AutoMapper;
using Multimedia.Images.Repositories.Interfaces;
using Multimedia.Images.Services.Interfaces;
using Multimedia.Images.Settings;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using Multimedia.Images.Dtos;
using Multimedia.Images.Exceptions;
using Multimedia.Images.Entities;

namespace Multimedia.Images.Services
{
    public class ImagesService : IImagesService
    {
        private readonly IImagesRepository _imagesRepository;
        private readonly DockerMediaRepositorySettings _mediaRepositorySettings;
        private readonly IMapper _mapper;

        public ImagesService(IImagesRepository imagesRepository, DockerMediaRepositorySettings mediaRepositorySettings, IMapper mapper)
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

        public async Task<List<ImageDto>> GetImagesByHashtag(HashtagDto hashtagDto)
        {
            var images = await _imagesRepository.GetImagesByHashtag(hashtagDto.Hashtag);

            return _mapper.Map<List<ImageDto>>(images);
        }

        public async Task<List<ImageDto>> GetUserImages(UserIdDto userIdDto)
        {
            if (String.IsNullOrEmpty(userIdDto.UserId))
                throw new ArgumentNullException(nameof(userIdDto.UserId));

            var images = await _imagesRepository.GetUserImages(Guid.Parse(userIdDto.UserId));

            return _mapper.Map<List<ImageDto>>(images);
        }

        public async Task<ImageDto> CreateImage(CommandImageDto commandImageDto, Stream stream, string extension)
        {
            var imageToAdd = _mapper.Map<Image>(commandImageDto);
            imageToAdd.CreationDate = DateTime.Now;

            var resourcePath = _mediaRepositorySettings.DockerImagePath + '/' + commandImageDto.UserId + '/' + Guid.NewGuid() + extension;
            imageToAdd.Path = resourcePath;
            var directoryPath = Path.GetDirectoryName(resourcePath);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            using (var fs = File.Create(resourcePath))
                stream.CopyTo(fs);

            var image = await _imagesRepository.AddImage(imageToAdd);

            return _mapper.Map<ImageDto>(image);
        }

        public async Task UpdateImage(int id, CommandImageDto commandImageDto)
        {
            var imageToUpdate = await _imagesRepository.GetUserImage(id, Guid.Parse(commandImageDto.UserId));

            if (imageToUpdate == null)
                throw new NotFoundException("Image with this id does not exist.");

            imageToUpdate = _mapper.Map<Image>(commandImageDto);

            await _imagesRepository.UpdateImage(id, imageToUpdate);
        }

        public async Task DeleteImage(int id, UserIdDto userIdDto)
        {
            if (String.IsNullOrEmpty(userIdDto.UserId))
                throw new ArgumentNullException(nameof(userIdDto.UserId));

            var imageToDelete = await _imagesRepository.GetUserImage(id, Guid.Parse(userIdDto.UserId));

            if (imageToDelete == null)
                throw new NotFoundException("Image with this id does not exist.");

            await _imagesRepository.DeleteImage(id, Guid.Parse(userIdDto.UserId));
        }
    }
}
