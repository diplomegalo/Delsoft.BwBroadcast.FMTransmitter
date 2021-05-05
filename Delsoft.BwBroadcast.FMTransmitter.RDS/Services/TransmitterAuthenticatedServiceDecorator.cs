using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http;
using System.Threading.Tasks;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static Delsoft.BwBroadcast.FMTransmitter.RDS.Utils.Constants.Transmitter;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Services
{
    public class TransmitterAuthenticatedServiceDecorator : ITransmitterService
    {
        private readonly ILogger<TransmitterAuthenticatedServiceDecorator> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TransmitterService _transmitterService;
        private readonly IOptions<TransmitterOptions> _options;

        public TransmitterAuthenticatedServiceDecorator(ILogger<TransmitterAuthenticatedServiceDecorator> logger, IHttpClientFactory httpClientFactory, TransmitterService transmitterService, IOptions<TransmitterOptions> options)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _transmitterService = transmitterService;
            _options = options;
        }

        public async Task SetRadioText(string nowPlaying)
        {
            var retry = 3;
            while (retry > 0)
            {
                try
                {
                    await _transmitterService.SetRadioText(nowPlaying);
                    break;
                }
                catch (Exception e)
                {
                    retry--;
                    _logger.LogWarning($"Unable to set radio text. Try to authenticate. Number of retry {retry}");

                    var httpClient = _httpClientFactory.CreateClient(_options);
                    httpClient.GetAsync(Routes.BuildAuthenticateUri(_options.Value.Password));
                    Task.Delay(500);
                }
            }

            if (retry == 0)
            {
                var message = $"Unable to set radio text to: {nowPlaying}.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }
        }

        public async Task<IEnumerable<string>> GetRadioText()
        {
            var retry = 3;
            while (retry > 0)
            {
                try
                {
                    return await _transmitterService.GetRadioText();
                }
                catch (Exception e)
                {
                    retry--;
                    _logger.LogWarning($"Unable to get radio text. Try to authenticate. Number of retry {retry}");

                    var httpClient = _httpClientFactory.CreateClient(_options);
                    httpClient.GetAsync(Routes.BuildAuthenticateUri(_options.Value.Password));
                    Task.Delay(500);
                }
            }

            var message = $"Unable to get radio text to.";
            _logger.LogError(message);
            throw new InvalidOperationException(message);
        }
    }
}