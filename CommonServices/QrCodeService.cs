using Bot.CommonServices.Converters;
using ColorZXing;
using System;
using System.Threading.Tasks;

namespace Bot.CommonServices.CommonServices
{
    public class QrcodeService : IQrcodeService
    {
        public async Task<QrResult> Decode(string imageUrl)
        {
            return await Task.Run(() => {
                return new QrResult() {
                    Text = ColorZXingBasic.Decode(new Uri(imageUrl))            
                };
            });
        }

        public async Task<T> Decode<T>(string imageUrl, IConverter<QrResult, T> converter)
        {
            var qrResult = await Decode(imageUrl);
            return converter.Convert(qrResult);
        }
    }
}
