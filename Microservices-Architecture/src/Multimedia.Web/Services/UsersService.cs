using Multimedia.Web.Dtos;
using Multimedia.Web.Services.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace Multimedia.Web.Services
{
    public class UsersService : BaseService, IUsersService
    {
        private readonly IHttpClientFactory _clientFactory;

        public UsersService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> CreateUser<T>(CreateUserDto newUserDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = newUserDto,
                Url = SD.UsersAPIBase + "/Account/Register",
                AccessToken = token
            });
        }

        public async Task<T> GetUserById<T>(UserIdDto userIdDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Data = userIdDto,
                Url = SD.UsersAPIBase + "/Account/User",
                AccessToken = token
            });
        }

        public async Task<T> GetUserByEmail<T>(UserEmailDto emailDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Data = emailDto,
                Url = SD.UsersAPIBase + "/Account/Email",
                AccessToken = token
            });
        }

        public async Task<T> LoginUser<T>(LoginUserDto loginUserDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = loginUserDto,
                Url = SD.UsersAPIBase + "/Account/Authenticate",
                AccessToken = token
            });
        }
    }
}
