using Multimedia.Web.Dtos;
using System;
using System.Threading.Tasks;

namespace Multimedia.Web.Services.Interfaces
{
    public interface IBaseService : IDisposable
    {
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
