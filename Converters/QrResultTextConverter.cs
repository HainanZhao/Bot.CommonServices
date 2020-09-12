using ZXing;

namespace Bot.CommonServices.Converters
{
    public class QrResultTextConverter : IConverter<string>
    {
        public string Convert(object original)
        {
            var result = original as Result;
            return result?.Text;
        }
    }
}
