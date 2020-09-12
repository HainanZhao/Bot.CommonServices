using Bot.CommonServices.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bot.CommonServices.CommonServices
{
    public interface IQrcodeService
    {
        public Task<T> Decode<T>(string imageUrl, IConverter<T> converter);
    }
}
