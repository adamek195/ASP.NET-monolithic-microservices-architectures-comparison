using AutoMapper;
using MonolithicMultimedia.Dtos;
using MonolithicMultimedia.Entities;
using MonolithicMultimedia.Exceptions;
using MonolithicMultimedia.Repositories;
using MonolithicMultimedia.Repositories.Interfaces;
using MonolithicMultimedia.Services.Interfaces;
using MonolithicMultimedia.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MonolithicMultimedia.Services
{
    public class VideosService : IVideosService
    {
        private readonly IVideosRepository _videosRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly MediaRepositorySettings _mediaRepositorySettings;
        private readonly IMapper _mapper;

        public VideosService(IVideosRepository videosRepository, IUsersRepository usersRepository, MediaRepositorySettings mediaRepositorySettings, IMapper mapper)
        {
            _videosRepository = videosRepository;
            _usersRepository = usersRepository;
            _mediaRepositorySettings = mediaRepositorySettings;
            _mapper = mapper;
        }

        public async Task CreateVideo(CommandVideoDto commandVideoDto, Stream stream, string userId, string fileName)
        {
            var videoToAdd = _mapper.Map<Video>(commandVideoDto);
            videoToAdd.CreationDate = DateTime.Now;
            videoToAdd.UserId = Guid.Parse(userId);

            var resourcePath = _mediaRepositorySettings.VideoPath + '\\' + userId + '\\' + fileName;
            videoToAdd.Path = resourcePath;
            var directoryPath = Path.GetDirectoryName(resourcePath);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            using (var fs = File.Create(resourcePath))
                stream.CopyTo(fs);

            await _videosRepository.AddVideo(videoToAdd);
        }

        public async Task DeleteVideo(int id, string userId)
        {
            if (String.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            var videoToDelete = await _videosRepository.GetUserVideo(id, Guid.Parse(userId));

            if (videoToDelete == null)
                throw new NotFoundException("Video with this id does not exist.");

            await _videosRepository.DeleteVideo(id, Guid.Parse(userId));
        }

        public async Task<List<VideoDto>> GetUserVideos(string userId)
        {
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

        public async Task<List<VideoDto>> GetVideosByEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));

            var user = await _usersRepository.GetUserByEmail(email);

            if (user == null)
                throw new NotFoundException("Not found user with this email");

            var videos = await _videosRepository.GetUserVideos(user.Id);

            return _mapper.Map<List<VideoDto>>(videos);
        }

        public async Task<List<VideoDto>> GetVideosByHashtag(string hashtag)
        {
            var videos = await _videosRepository.GetVideosByHashtag(hashtag);

            return _mapper.Map<List<VideoDto>>(videos);
        }

        public async Task UpdateVideo(int id, string userId, CommandVideoDto commandVideoDto)
        {
            if (String.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            var videoToUpdate = await _videosRepository.GetUserVideo(id, Guid.Parse(userId));

            if (videoToUpdate == null)
                throw new NotFoundException("Video with this id does not exist.");

            videoToUpdate = _mapper.Map<Video>(commandVideoDto);
            videoToUpdate.UserId = Guid.Parse(userId);

            await _videosRepository.UpdateVideo(id, videoToUpdate);
        }
    }
}
