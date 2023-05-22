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
    public class ImagesService : BaseService, IImagesService
    {
        private readonly IHttpClientFactory _clientFactory;

        public ImagesService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task CreateImage(CommandImageDto commandImageDto, Stream stream, string fileName, string token = null)
        {
            var client = httpClient.CreateClient("MultimediaAPI");
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, "file");
            message.RequestUri = new Uri(SD.APIBase + $"/Image");
            client.DefaultRequestHeaders.Clear();

            using var content = new MultipartFormDataContent
            {
                { new StreamContent(stream), "ImageFile", fileName },
                { new StringContent(commandImageDto.Title), "CommandImageDto.Title" },
                { new StringContent(commandImageDto.Description), "CommandImageDto.Description" },
                { new StringContent(commandImageDto.UserId.ToString()), "CommandImageDto.UserId" },
                { new StringContent(commandImageDto.Hashtag), "CommandImageDto.Hashtag" }
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
            var image = JsonConvert.DeserializeObject<ImageDto>(apiContent);

            if (image == null)
                throw new BadRequestException("Image was not created");
        }

        public async Task<T> DeleteImage<T>(int id, UserIdDto userIdDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.APIBase + $"/Image/{id}",
                Data = userIdDto,
                AccessToken = token
            });
        }

        public async Task<T> GetImage<T>(int id, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.APIBase + $"/Image/{id}",
                AccessToken = token
            });
        }

        public async Task<T> GetImages<T>(string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.APIBase + "/Image",
                AccessToken = token
            });
        }

        public async Task<T> GetImagesByHashtag<T>(HashtagDto hashtagDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.APIBase + $"/Image/Hashtag",
                Data = hashtagDto,
                AccessToken = token
            });
        }

        public async Task<T> GetUserImages<T>(UserIdDto userIdDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.APIBase + "/Image/User",
                Data = userIdDto,
                AccessToken = token
            });
        }

        public async Task<T> UpdateImage<T>(int id, CommandImageDto commandImageDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.PUT,
                Url = SD.APIBase + $"/Image/{id}",
                Data = commandImageDto,
                AccessToken = token
            });
        }
    }
}
