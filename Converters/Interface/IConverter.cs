using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.CommonServices.Converters
{
    public interface IConverter<T>
    {
        public T Convert(object original);
    }
}
