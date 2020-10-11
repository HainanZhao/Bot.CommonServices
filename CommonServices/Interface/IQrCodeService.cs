using Bot.CommonServices.Converters;
using System.Threading.Tasks;

namespace Bot.CommonServices
{
    public class QrResult
    {
        public string Text { get; set; }
        public byte[] RawByte { get; set; }
        public long Timestamp { get; set; }
    }

    public interface IQrcodeService
    {
        public Task<QrResult> Decode(string imageUrl);
        public Task<T> Decode<T>(string imageUrl, IConverter<QrResult, T> converter);
    }
}
