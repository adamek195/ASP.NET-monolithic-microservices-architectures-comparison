using AutoMapper;
using Multimedia.Videos.Entities;
using Multimedia.Videos.Repositories.Interfaces;
using Multimedia.Videos.Services.Interfaces;
using Multimedia.Videos.Settings;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using Multimedia.Videos.Dtos;
using Multimedia.Videos.Exceptions;

namespace Multimedia.Videos.Services
{
    public class VideosService : IVideosService
    {
        private readonly IVideosRepository _videosRepository;
        private readonly MediaRepositorySettings _mediaRepositorySettings;
        private readonly IMapper _mapper;

        public VideosService(IVideosRepository videosRepository, MediaRepositorySettings mediaRepositorySettings, IMapper mapper)
        {
            _videosRepository = videosRepository;
            _mediaRepositorySettings = mediaRepositorySettings;
            _mapper = mapper;
        }

        public async Task<VideoDto> CreateVideo(CommandVideoDto commandVideoDto, Stream stream, string fileName)
        {
            var videoToAdd = _mapper.Map<Video>(commandVideoDto);
            videoToAdd.CreationDate = DateTime.Now;

            var resourcePath = _mediaRepositorySettings.VideoPath + '\\' + commandVideoDto.UserId + '\\' + fileName;
            videoToAdd.Path = resourcePath;
            var directoryPath = Path.GetDirectoryName(resourcePath);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            using (var fs = File.Create(resourcePath))
                stream.CopyTo(fs);

            var video = await _videosRepository.AddVideo(videoToAdd);

            return _mapper.Map<VideoDto>(video);
        }

        public async Task DeleteVideo(int id, UserDto userDto)
        {
            var videoToDelete = await _videosRepository.GetUserVideo(id, userDto.UserId);

            if (videoToDelete == null)
                throw new NotFoundException("Video with this id does not exist.");

            await _videosRepository.DeleteVideo(id, userDto.UserId);
        }

        public async Task<List<VideoDto>> GetUserVideos(string userId)
        {
            if (String.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            var videos = await _videosRepository.GetUserVideos(Guid.Parse(userId));

            return _mapper.Map<List<VideoDto>>(videos);
        }

        public async Task<VideoDto> GetVideo(int id)
        {
            var video = await _videosRepository.GetVideo(id);

            if (video == null)
                throw new NotFoundException("Video with this id does not exist.");

            return _mapper.Map<VideoDto>(video);
        }

        public async Task<List<VideoDto>> GetVideos()
        {
            var videos = await _videosRepository.GetVideos();

            return _mapper.Map<List<VideoDto>>(videos);
        }

        public async Task<List<VideoDto>> GetVideosByHashtag(string hashtag)
        {
            if (String.IsNullOrEmpty(hashtag))
                throw new ArgumentNullException(nameof(hashtag));

            var videos = await _videosRepository.GetVideosByHashtag(hashtag);

            return _mapper.Map<List<VideoDto>>(videos);
        }

        public async Task UpdateVideo(int id, CommandVideoDto commandVideoDto)
        {
            var videoToUpdate = await _videosRepository.GetUserVideo(id, commandVideoDto.UserId);

            if (videoToUpdate == null)
                throw new NotFoundException("Video with this id does not exist.");

            videoToUpdate = _mapper.Map<Video>(commandVideoDto);

            await _videosRepository.UpdateVideo(id, videoToUpdate);
        }
    }
}
