using System.Net.Http;

namespace ElementMusic.Models
{
    internal abstract class BaseAPISender : IAPISender
    {
        protected Uri _baseUrl;
        protected HttpClient _httpClient;

        public BaseAPISender(Uri baseUrl)
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = _baseUrl;
        }

        ~BaseAPISender() =>
            _httpClient.Dispose();

        public virtual Task<HttpResponseMessage?> SendRequest(string endpoint, HttpMethod method, Dictionary<string, object> parameters) =>
            throw new NotImplementedException();

        public void AddRequestHeader(string key, string value)
        {
            if (_httpClient.DefaultRequestHeaders.Contains(key))
                RemoveRequestHeader(key);
            _httpClient.DefaultRequestHeaders.Add(key, value);
        }

        public void RemoveRequestHeader(string key) => _httpClient.DefaultRequestHeaders.Remove(key);
    }

    internal interface IAPISender
    {
        public Task<HttpResponseMessage?> SendRequest(string endpoint, HttpMethod method, Dictionary<string, object> parameters);
        public void AddRequestHeader(string key, string value);
        public void RemoveRequestHeader(string key);
    }
}
