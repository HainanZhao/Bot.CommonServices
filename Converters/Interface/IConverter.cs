using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.CommonServices.Converters
{
    public interface IConverter<TOriginal, T>
    {
        public T Convert(TOriginal original);
    }
}
