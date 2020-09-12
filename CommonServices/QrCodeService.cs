using Bot.CommonServices.Converters;
using Bot.CommonServices.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;
using ZXing.CoreCompat.System.Drawing;

namespace Bot.CommonServices.CommonServices
{
    public class QrcodeService : IQrcodeService
    {    
        public async Task<T> Decode<T>(string imageUrl, IConverter<T> converter)
        {
            return await Task.Run(() => {

                var bytes = Utilities.GetImageAsByteArray(imageUrl);
                using (var ms = new MemoryStream(bytes))
                {
                    var image = new Bitmap(ms);
                    var source = new BitmapLuminanceSource(image);
                    BinaryBitmap bitmap = new BinaryBitmap(new HybridBinarizer(source));
                    Result qrResult = new MultiFormatReader().decode(bitmap);

                    return converter.Convert(qrResult);
                }                
            });
        }
    }
}
