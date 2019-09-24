using System;
using System.Collections.Generic;
using System.Web;

namespace Jmwoo.Common.Http
{
    public static class HttpHelpers
    {
        public static string BuildUrl(string domain, string path, Dictionary<string, string> queryParams = null, bool useHttps = true)
        {
            var scheme = useHttps ? HttpScheme.Https : HttpScheme.Http;
            var port = scheme.Value == HttpScheme.Https.Value ? 443 : 80;

            var uriBuilder = new UriBuilder(scheme.Value, domain, port)
            {
                Path = path
            };

            if (queryParams != null)
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);

                foreach (var kv in queryParams)
                {
                    query[kv.Key] = kv.Value;
                }

                uriBuilder.Query = query.ToString();
            }

            return uriBuilder.Uri.ToString();
        }

        public class HttpScheme
        {
            public string Value { get; }

            private HttpScheme(string value)
            {
                Value = value;
            }

            public static HttpScheme Http => new HttpScheme("http");
            public static HttpScheme Https => new HttpScheme("https");
        }
    }
}
