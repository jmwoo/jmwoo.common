using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Jmwoo.Common.Http
{
    public static class HttpClientExtensions
    {
        public static async Task<T> SendAndReceiveAs<T>(this HttpClient httpClient, HttpMethod httpMethod, string requestUri, object content = null)
        {
            var contentString = await httpClient.SendAndReceiveAsString(httpMethod, requestUri, content);
            return JsonConvert.DeserializeObject<T>(contentString);
        }

        public static async Task<string> SendAndReceiveAsString(this HttpClient httpClient, HttpMethod httpMethod, string requestUri, object content = null)
        {
            var res = await httpClient.SendAndReceiveAsHttpMessage(httpMethod, requestUri, content);
            return await res.Content.ReadAsStringAsync();
        }

        public static async Task<HttpResponseMessage> SendAndReceiveAsHttpMessage(this HttpClient httpClient, HttpMethod httpMethod, string requestUri, object content = null, bool ensureSuccess = true)
        {
            var httpMessage = await httpClient.SendAsync(new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = new Uri(requestUri),
                Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json")
            });

            if (ensureSuccess)
            {
                httpMessage.EnsureSuccessStatusCode();
            }

            return httpMessage;
        }
    }
}
