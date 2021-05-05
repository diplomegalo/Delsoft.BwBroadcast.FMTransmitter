using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Model;
using Microsoft.Extensions.Options;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Utils
{
    public static class HttpClientExtension
    {
        public static async Task<TReturn> GetAsync<TReturn>(this HttpClient httpClient, string url)
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            using var reader = XmlReader.Create(new StringReader(content));
            return (TReturn)new XmlSerializer(typeof(TReturn)).Deserialize(reader);
        }

        public static HttpClient CreateClient(this IHttpClientFactory httpClientFactory, IOptions<TransmitterOptions> options)
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(options.Value.Endpoint);

            return httpClient;
        }
    }
}