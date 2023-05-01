using Microsoft.EntityFrameworkCore;
using MonolithicMultimedia.Data;
using MonolithicMultimedia.Entities;
using MonolithicMultimedia.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MonolithicMultimedia.Repositories
{
    public class VideosRepository : IVideosRepository
    {
        private readonly MonolithicMultimediaContext _context;

        public VideosRepository(MonolithicMultimediaContext context)
        {
            _context = context;
        }

        public async Task AddVideo(Video video)
        {
            _context.Videos.Add(video);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteVideo(int id, Guid userId)
        {
            var videoToDelete = await _context.Videos.SingleOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (videoToDelete != null)
                _context.Videos.Remove(videoToDelete);

            await _context.SaveChangesAsync();
        }

        public async Task<Video> GetUserVideo(int id, Guid userId)
        {
            var video = await _context.Videos.SingleOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            return video;
        }

        public async Task<List<Video>> GetUserVideos(Guid userId)
        {
            var videos = await _context.Videos.Where(x => x.UserId == userId).OrderByDescending(x => x.CreationDate).ToListAsync();

            return videos;
        }

        public async Task<Video> GetVideo(int id)
        {
            var video = await _context.Videos.SingleOrDefaultAsync(x => x.Id == id);

            return video;
        }

        public async Task<List<Video>> GetVideos()
        {
            var videos = await _context.Videos.OrderByDescending(x => x.CreationDate).ToListAsync();

            return videos;
        }

        public async Task<List<Video>> GetVideosByHashtag(string hashtag)
        {
            var videos = await _context.Videos.Where(x => x.Hashtag.Contains(hashtag)).OrderByDescending(x => x.CreationDate).ToListAsync();

            return videos;
        }

        public async Task UpdateVideo(int id, Video video)
        {
            var videoToUpdate = await _context.Videos.SingleOrDefaultAsync(x => x.Id == id && x.UserId == video.UserId);

            if (videoToUpdate != null)
            {
                videoToUpdate.Title = video.Title;
                videoToUpdate.Description = video.Description;
                videoToUpdate.Hashtag = video.Hashtag;
            }

            await _context.SaveChangesAsync();
        }
    }
}
