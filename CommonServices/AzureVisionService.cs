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

        public async Task<T> AnalyzeImageAsync<T>(string imageUrl, IConverter<T> beautifier)
        {
            try
            {
                var visualFeatures = new List<VisualFeatureTypes>()
                {
                    VisualFeatureTypes.Categories,
                    VisualFeatureTypes.Tags,
                    VisualFeatureTypes.Description,
                    VisualFeatureTypes.Faces,
                    VisualFeatureTypes.Brands,
                    VisualFeatureTypes.Objects
                };
                var client = new WebClient();
                using Stream stream = client.OpenRead(imageUrl);
                ImageAnalysis analysisResult = await _visionClient.AnalyzeImageInStreamAsync(stream, visualFeatures);

                return beautifier.Convert(analysisResult);                    
            }
            catch (Exception)
            {
                return default;
            }           
        }

        public async Task<T> ReadTextFromImageAsyc<T>(string imageUrl, IConverter<T> converter)
        {
            try
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
                return converter.Convert(result);
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
