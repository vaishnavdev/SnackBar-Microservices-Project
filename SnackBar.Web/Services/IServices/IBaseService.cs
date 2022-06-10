using SnackBar.Web.Models;

namespace SnackBar.Web.Services.IServices
{
    public interface IBaseService : IDisposable
    {
        ResponseDto responseModel { get; set; }
        Task<T> sendAsync<T>(ApiRequest apiRequest);
    }
}
