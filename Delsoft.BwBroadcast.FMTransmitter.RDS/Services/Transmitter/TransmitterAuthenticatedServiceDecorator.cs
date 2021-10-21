using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Utils.Extensions;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Utils.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static Delsoft.BwBroadcast.FMTransmitter.RDS.Utils.Constants.Transmitter;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Transmitter
{
    public class TransmitterAuthenticatedServiceDecorator : ITransmitterService
    {
        private readonly ILogger<TransmitterAuthenticatedServiceDecorator> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TransmitterService _transmitterService;
        private readonly IOptions<TransmitterServiceOptions> _options;

        public TransmitterAuthenticatedServiceDecorator(ILogger<TransmitterAuthenticatedServiceDecorator> logger, IHttpClientFactory httpClientFactory, TransmitterService transmitterService, IOptions<TransmitterServiceOptions> options)
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
                    await _transmitterService.SetRadioText(nowPlaying)
                        .ConfigureAwait(true);
                    break;
                }
                catch (InvalidOperationException)
                {
                    retry--;
                    _logger.LogTrace($"Try to authenticate. Number of retry {retry}");

                    var httpClient = _httpClientFactory.CreateClient(_options);
                    await httpClient.GetAsync(Routes.BuildAuthenticateUri(_options.Value.Password)).ConfigureAwait(true);
                    await Task.Delay(500);
                }
                catch (HttpRequestException e)
                {
                    if (e.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        _logger.LogTrace($"Try to authenticate. Number of retry {retry}");
                        retry--;

                        var httpClient = _httpClientFactory.CreateClient(_options);
                        await httpClient.GetAsync(Routes.BuildAuthenticateUri(_options.Value.Password));
                        await Task.Delay(500);
                    }
                }
            }

            if (retry == 0)
            {
                throw new InvalidOperationException($"Unable to set radio text to: {nowPlaying}.");
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
                catch (InvalidOperationException)
                {
                    _logger.LogTrace($"Try to authenticate. Number of retry {retry}");
                    retry--;

                    var httpClient = _httpClientFactory.CreateClient(_options);
                    await httpClient.GetAsync(Routes.BuildAuthenticateUri(_options.Value.Password));
                    await Task.Delay(500);
                }
                catch (HttpRequestException e)
                {
                    if (e.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        _logger.LogTrace($"Try to authenticate. Number of retry {retry}");
                        retry--;

                        var httpClient = _httpClientFactory.CreateClient(_options);
                        await httpClient.GetAsync(Routes.BuildAuthenticateUri(_options.Value.Password));
                        await Task.Delay(500);
                    }
                }
            }

            throw new InvalidOperationException($"Unable to get radio text to.");
        }
    }
}