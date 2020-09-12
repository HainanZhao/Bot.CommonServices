using Bot.CommonServices.CommonServices;

namespace Bot.CommonServices.Converters
{
    public class QrResultTextConverter : IConverter<QrResult, string>
    {
        public string Convert(QrResult original)
        {            
            return original?.Text;
        }
    }
}
