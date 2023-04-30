using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MonolithicMultimedia.Entities;

namespace MonolithicMultimedia.Repositories.Interfaces
{
    public interface IVideosRepository
    {
        Task<Video> GetVideo(int id);

        Task<Video> GetUserVideo(int id, Guid userId);

        Task<List<Video>> GetVideos();

        Task<List<Video>> GetVideosByHashtag(string hashtag);

        Task<List<Video>> GetUserVideos(Guid userId);

        public Task AddVideo(Video video);

        public Task UpdateVideo(int id, Video video);

        public Task DeleteVideo(int id, Guid userId);
    }
}
