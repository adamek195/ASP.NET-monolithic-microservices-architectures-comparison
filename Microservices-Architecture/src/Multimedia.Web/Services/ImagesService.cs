using Multimedia.Web.Dtos;
using Multimedia.Web.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using static Multimedia.Web.SD;

namespace Multimedia.Web.Services
{
    public class ImagesService : BaseService, IImagesService
    {
        private readonly IHttpClientFactory _clientFactory;

        public ImagesService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public Task<T> CreateImage<T>(CommandImageDto commandImageDto, Stream stream, string userId, string fileName, string token = null)
        {
            throw new System.NotImplementedException();
        }

        public async Task<T> DeleteImage<T>(int id, UserIdDto userIdDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.ImagesAPIBase + $"/Image/{id}",
                Data = userIdDto,
                AccessToken = token
            });
        }

        public async Task<T> GetImage<T>(int id, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ImagesAPIBase + $"/Image/{id}",
                AccessToken = token
            });
        }

        public async Task<T> GetImages<T>(string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ImagesAPIBase + "/Image",
                AccessToken = token
            });
        }

        public async Task<T> GetImagesByHashtag<T>(HashtagDto hashtagDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ImagesAPIBase + $"/Image/Hashtag",
                Data = hashtagDto,
                AccessToken = token
            });
        }

        public async Task<T> GetUserImages<T>(UserIdDto userIdDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ImagesAPIBase + "/Image/User",
                Data = userIdDto,
                AccessToken = token
            });
        }

        public async Task<T> UpdateImage<T>(int id, CommandImageDto commandImageDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.PUT,
                Url = SD.ImagesAPIBase + $"/Image/{id}",
                Data = commandImageDto,
                AccessToken = token
            });
        }
    }
}
