using System.IO;
using System.Net.Http;

namespace ElementMusic.Models.ElementAPI
{
    internal class APISender
    {
        private Uri _baseUrl = new Uri("https://elemsocial.com/System/API/");
        private HttpClient _httpClient = new HttpClient();

        public APISender()
        {
            _httpClient.BaseAddress = _baseUrl;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "ElementAPI");
            _httpClient.DefaultRequestHeaders.ExpectContinue = false;

            if (Properties.Settings.Default.SessionKey != null)
                _httpClient.DefaultRequestHeaders.Add("S-KEY", Properties.Settings.Default.SessionKey);

            Properties.Settings.Default.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == nameof(Properties.Settings.Default.SessionKey)
                && Properties.Settings.Default.SessionKey != null)
                    _httpClient.DefaultRequestHeaders.Add("S-KEY", Properties.Settings.Default.SessionKey);
            };
        }

        ~APISender()
        {
            _httpClient.Dispose();
        }

        public async Task<HttpResponseMessage?> SendRequest(string endpoint, HttpMethod method, Dictionary<string, object> parameters = null)
        {
            using (var formData = new MultipartFormDataContent())
            {
                if (parameters != null)
                    foreach (var item in parameters)
                    {
                        if (item.Value is string s)
                            if (File.Exists(s))
                            {
                                var fileStream = File.OpenRead(s);
                                var streamContent = new StreamContent(fileStream);
                                formData.Add(streamContent, item.Key, Path.GetFileName(s));
                            }
                            else
                                formData.Add(new StringContent(s), item.Key);
                        else
                            formData.Add(new StringContent(Convert.ToString(item.Value)), item.Key);
                    }

                switch (method.Method.ToLower())
                {
                    case "post":
                        var respPost = await _httpClient.PostAsync(endpoint, formData);
                        return respPost;
                    case "get":
                        var respGet = await _httpClient.GetAsync(endpoint);
                        return respGet;
                    default: 
                        return null;
                }
            }
        }

        public void AddRequestHeader(string key, string value)
        {
            if (_httpClient.DefaultRequestHeaders.Contains(key))
                RemoveRequestHeader(key);
            _httpClient.DefaultRequestHeaders.Add(key, value);
        }

        public void RemoveRequestHeader(string key) => _httpClient.DefaultRequestHeaders.Remove(key);
    }
}
