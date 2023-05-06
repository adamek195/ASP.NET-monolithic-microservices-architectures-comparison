using Multimedia.Web.Dtos;
using Multimedia.Web.Services.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Multimedia.Web.Exceptions;

namespace Multimedia.Web.Services
{
    public class BaseService : IBaseService
    {
        public IHttpClientFactory httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            var client = httpClient.CreateClient("MultimediaAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(apiRequest.Url);
            client.DefaultRequestHeaders.Clear();
            if (apiRequest.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                    Encoding.UTF8, "application/json");
            }

            if (!string.IsNullOrEmpty(apiRequest.AccessToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);
            }

            HttpResponseMessage apiResponse = null;
            switch (apiRequest.ApiType)
            {
                case SD.ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case SD.ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case SD.ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;
            }
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
            var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
            return apiResponseDto;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
