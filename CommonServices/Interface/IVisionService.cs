using Bot.CommonServices.Converters;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot.CommonServices
{
    public interface IVisionService
    {
        public Task<ImageAnalysis> AnalyzeImageAsync(string imageUrl, IList<VisualFeatureTypes> features);
        public Task<T> AnalyzeImageAsync<T>(string imageUrl, IList<VisualFeatureTypes> features, IConverter<ImageAnalysis, T> converter);

        public Task<OcrResult> ReadTextFromImageAsyc(string imageUrl);
        public Task<T> ReadTextFromImageAsyc<T>(string imageUrl, IConverter<OcrResult, T> beautifier);
    }
}
