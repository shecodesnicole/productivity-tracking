using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;

namespace Pd.Gateway.Application.Domain.Services.Http
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient httpClient;
        private readonly IHttpContextAccessor httpContextAccessor;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        public HttpService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClient = httpClient;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResult> GetAsync<TResult>(string url)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            ForwardAuthHeader(request);
            using var response = await httpClient.SendAsync(request);
            return await DeserializeAsync<TResult>(response);
        }

        public async Task<TResult> PostAsync<TResult>(string url, object payload)
        {
            var json = JsonSerializer.Serialize(payload, JsonOptions);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };
            ForwardAuthHeader(request);
            using var response = await httpClient.SendAsync(request);
            return await DeserializeAsync<TResult>(response);
        }

        public async Task<TResult> PutAsync<TResult>(string url, object payload)
        {
            var json = JsonSerializer.Serialize(payload, JsonOptions);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var request = new HttpRequestMessage(HttpMethod.Put, url) { Content = content };
            ForwardAuthHeader(request);
            using var response = await httpClient.SendAsync(request);
            return await DeserializeAsync<TResult>(response);
        }

        public async Task<TResult> DeleteAsync<TResult>(string url)
        {
            using var request = new HttpRequestMessage(HttpMethod.Delete, url);
            ForwardAuthHeader(request);
            using var response = await httpClient.SendAsync(request);
            return await DeserializeAsync<TResult>(response);
        }

        private void ForwardAuthHeader(HttpRequestMessage request)
        {
            var authHeader = httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();
            if (!string.IsNullOrEmpty(authHeader))
                request.Headers.TryAddWithoutValidation("Authorization", authHeader);
        }

        private static async Task<TResult> DeserializeAsync<TResult>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
                return default!;
            try
            {
                return JsonSerializer.Deserialize<TResult>(json, JsonOptions)!;
            }
            catch (JsonException)
            {
                return default!;
            }
        }
    }
}
