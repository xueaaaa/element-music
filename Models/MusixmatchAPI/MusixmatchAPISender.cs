using System.Net.Http;

namespace ElementMusic.Models.MusixmatchAPI
{
    internal class MusixmatchAPISender : BaseAPISender
    {
        public MusixmatchAPISender() : base(new Uri("https://paxsenixofc.my.id/server/")) { }

        public override async Task<HttpResponseMessage?> SendRequest(string endpoint, HttpMethod method, Dictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                endpoint += "?";
                foreach (var param in parameters)
                    endpoint += $"{param.Key}={param.Value}&";

                switch (method.Method.ToLower())
                {
                    case "get":
                        var respget = await _httpClient.GetAsync(endpoint);
                        return respget;
                    default:
                        return null;
                }
            }
            else return null;
        }
    }
}
