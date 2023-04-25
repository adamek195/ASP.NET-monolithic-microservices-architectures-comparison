using AutoMapper;
using MonolithicMultimedia.Dtos;
using MonolithicMultimedia.Entities;
using MonolithicMultimedia.Repositories.Interfaces;
using MonolithicMultimedia.Services.Interfaces;
using MonolithicMultimedia.Settings;
using System;
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

        public async Task CreateImage(CommandImageDto commandImageDto, Stream stream, string userId, string fileName)
        {
            var imageToAdd = _mapper.Map<Image>(commandImageDto);
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
    }
}
