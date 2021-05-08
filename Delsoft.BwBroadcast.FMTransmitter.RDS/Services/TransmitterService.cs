using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Utils;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Model;
using Microsoft.Extensions.Logging;
using static Delsoft.BwBroadcast.FMTransmitter.RDS.Utils.Constants.Transmitter;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Services
{
    public class TransmitterService : ITransmitterService
    {
        private readonly ILogger<TransmitterService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<TransmitterOptions> _options;

        public TransmitterService(ILogger<TransmitterService> logger, IHttpClientFactory httpClientFactory, IOptions<TransmitterOptions> options)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _options = options;
        }

        public async Task SetRadioText(string nowPlaying)
        {
            if (string.IsNullOrWhiteSpace(nowPlaying))
            {
                throw new ArgumentNullException(nameof(nowPlaying));
            }

            using var httpClient = _httpClientFactory.CreateClient(_options);
            var response = await httpClient.GetAsync<Response>(
                Routes.BuildSetParameterUri(Parameters.RadioText, nowPlaying));

            if (!response.Success)
            {
                throw new InvalidOperationException(response.Reason);
            }
        }

        public async Task<IEnumerable<string>> GetRadioText()
        {
            using var httpClient = _httpClientFactory.CreateClient(_options);
            var httpResponseMessage = await httpClient.GetAsync(
                Routes.BuildGetParameterUri(Parameters.RadioText));

            httpResponseMessage.EnsureSuccessStatusCode();

            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            // Parse xml
            var xDocument = XDocument.Parse(content);
            if (xDocument.Root.Name.LocalName != "response")
            {
                var parameters = Deserialize<ParametersElem>(content)?.Parameters;
                var value = parameters?.Select(parameter => parameter.Value);
                return value;
            }

            var response = Deserialize<Response>(content);
            throw new InvalidOperationException(response.Reason);
        }

        private static T Deserialize<T>(string xml)
        {
            using var reader = XmlReader.Create(new StringReader(xml));
            return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
        }
    }
}