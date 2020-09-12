using Bot.CommonServices.Converters;
using System;
using System.Threading.Tasks;

namespace Bot.CommonServices
{
    public interface IVisionService
    {
        public Task<T> AnalyzeImageAsync<T>(string imageUrl, IConverter<T> converter);

        public Task<T> ReadTextFromImageAsyc<T>(string imageUrl, IConverter<T> beautifier);
    }
}
