using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Model;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Utils
{
    public static class HttpClientExtension
    {
        public static async Task<TReturn> GetAsync<TReturn>(this HttpClient httpClient, Uri uri)
        {
            var response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            using var reader = XmlReader.Create(new StringReader(content));
            return (TReturn)new XmlSerializer(typeof(TReturn)).Deserialize(reader);
        }
    }
}