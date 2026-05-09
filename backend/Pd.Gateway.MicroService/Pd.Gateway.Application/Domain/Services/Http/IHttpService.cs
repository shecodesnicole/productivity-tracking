namespace Pd.Gateway.Application.Domain.Services.Http
{
    public interface IHttpService
    {
        Task<TResult> GetAsync<TResult>(string url);
        Task<TResult> PostAsync<TResult>(string url, object payload);
        Task<TResult> PutAsync<TResult>(string url, object payload);
        Task<TResult> DeleteAsync<TResult>(string url);
    }
}
