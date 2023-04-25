﻿using MonolithicMultimedia.Entities;
using System.Collections.Generic;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MonolithicMultimedia.Repositories.Interfaces
{
    public interface IImagesRepository
    {
        Task<Image> GetImage(int id);

        Task<List<Image>> GetImages();

        Task<List<Image>> GetImagesByHashtag(string hashtag);

        Task<List<Image>> GetUserImages(Guid userId);

        public Task<Image> AddImage(Image image);
    }
}