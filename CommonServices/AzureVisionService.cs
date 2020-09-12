using Bot.CommonServices.Converters;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Bot.CommonServices
{
    public class AzureVisionService: IVisionService
    {
        private ComputerVisionClient _visionClient;

        /// <summary>
        /// Creates a Azure computer vision client. Make sure the "VisionServiceUrl" and "VisionSubscriptionKey" keys are in your configuration.
        /// </summary>
        /// <param name="configuration"></param>
        public AzureVisionService(IConfiguration configuration)
        {
            var endpoint = configuration["VisionServiceUrl"]?.ToString();
            var key = configuration["VisionSubscriptionKey"]?.ToString();
                _visionClient = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }

        public AzureVisionService(string endpoint, string key)
        {
            _visionClient = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }

        public async Task<ImageAnalysis> AnalyzeImageAsync(string imageUrl, IList<VisualFeatureTypes> features)
        {
            var client = new WebClient();
            using Stream stream = client.OpenRead(imageUrl);
            return await _visionClient.AnalyzeImageInStreamAsync(stream, features);                

        }

        public async Task<T> AnalyzeImageAsync<T>(string imageUrl, IList<VisualFeatureTypes> features, IConverter<ImageAnalysis, T> beautifier)
        {
            try
            {
                var analysisResult = await AnalyzeImageAsync(imageUrl, features);
                return beautifier.Convert(analysisResult);                    
            }
            catch (Exception)
            {
                return default;
            }           
        }

        public async Task<OcrResult> ReadTextFromImageAsyc(string imageUrl)
        {
            OcrResult result = null;
            if (new Uri(imageUrl).Host.ToLower() == "localhost")
            {
                var client = new WebClient();
                using Stream stream = client.OpenRead(imageUrl);
                result = await _visionClient.RecognizePrintedTextInStreamAsync(true, stream);
            }
            else
            {
                result = await _visionClient.RecognizePrintedTextAsync(true, imageUrl);
            }

            return result;
        }

        public async Task<T> ReadTextFromImageAsyc<T>(string imageUrl, IConverter<OcrResult, T> converter)
        {
            try
            {
                var result = await ReadTextFromImageAsyc(imageUrl);
                return converter.Convert(result);
            }
            catch (Exception)
            {
                return default;
            }
        }

    }
}
