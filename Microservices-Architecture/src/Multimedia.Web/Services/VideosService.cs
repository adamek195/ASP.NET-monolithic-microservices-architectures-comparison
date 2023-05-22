using Multimedia.Web.Dtos;
using Multimedia.Web.Exceptions;
using Multimedia.Web.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Multimedia.Web.Services
{
    public class VideosService : BaseService, IVideosService
    {
        private readonly IHttpClientFactory _clientFactory;

        public VideosService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task CreateVideo(CommandVideoDto commandVideoDto, Stream stream, string fileName, string token = null)
        {
            var client = httpClient.CreateClient("MultimediaAPI");
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, "file");
            message.RequestUri = new Uri(SD.APIBase + $"/Video");
            client.DefaultRequestHeaders.Clear();

            using var content = new MultipartFormDataContent
            {
                { new StreamContent(stream), "VideoFile", fileName },
                { new StringContent(commandVideoDto.Title), "CommandVideoDto.Title" },
                { new StringContent(commandVideoDto.Description), "CommandVideoDto.Description" },
                { new StringContent(commandVideoDto.UserId.ToString()), "CommandVideoDto.UserId" },
                { new StringContent(commandVideoDto.Hashtag), "CommandVideoDto.Hashtag" }
            };
            message.Content = content;

            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage apiResponse = null;
            apiResponse = await client.SendAsync(message);

            if (apiResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new NotFoundException(await apiResponse.Content.ReadAsStringAsync());
            if (apiResponse.StatusCode == System.Net.HttpStatusCode.Conflict)
                throw new ConflictException(await apiResponse.Content.ReadAsStringAsync());
            if (apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                throw new BadRequestException(await apiResponse.Content.ReadAsStringAsync());
            if (apiResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedException(await apiResponse.Content.ReadAsStringAsync());
            if (apiResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                throw new Exception(await apiResponse.Content.ReadAsStringAsync());

            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            var video = JsonConvert.DeserializeObject<VideoDto>(apiContent);

            if (video == null)
                throw new BadRequestException("Video was not created");
        }

        public async Task<T> DeleteVideo<T>(int id, UserIdDto userIdDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.APIBase + $"/Video/{id}",
                Data = userIdDto,
                AccessToken = token
            });
        }

        public async Task<T> GetVideo<T>(int id, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.APIBase + $"/Video/{id}",
                AccessToken = token
            });
        }

        public async Task<T> GetUserVideos<T>(UserIdDto userIdDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.APIBase + "/Video",
                AccessToken = token
            });
        }

        public async Task<T> GetVideos<T>(string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.APIBase + "/Video",
                AccessToken = token
            });
        }

        public async Task<T> GetVideosByHashtag<T>(HashtagDto hashtagDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.APIBase + $"/Video/Hashtag",
                Data = hashtagDto,
                AccessToken = token
            });
        }

        public async Task<T> UpdateVideo<T>(int id, CommandVideoDto commandVideoDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.PUT,
                Url = SD.APIBase + $"/Video/{id}",
                Data = commandVideoDto,
                AccessToken = token
            });
        }
    }
}
