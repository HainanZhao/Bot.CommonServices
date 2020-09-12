using Bot.CommonServices.Converters;
using System;
using System.Threading.Tasks;
using ColorZXing;

namespace Bot.CommonServices.CommonServices
{
    public class ColorQrcodeService : IQrcodeService
    {
        public async Task<QrResult> Decode(string imageUrl)
        {
            return await Task.Run(() =>
            {                
                return new QrResult()
                {
                    Text = ColorZXingRGB.Decode(new Uri(imageUrl))
                };
            });        
        }

        public async Task<T> Decode<T>(string imageUrl, IConverter<QrResult, T> converter)
        {
            var result = await Decode(imageUrl);
            return converter.Convert(result);
        }
    }
}
