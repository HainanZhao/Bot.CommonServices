using System;
using System.Net;
using System.Text;

namespace Bot.CommonServices.Utils
{
    public class Utilities
    {
        public static byte[] GetImageAsByteArray(string imageUrl)
        {
            var webClient = new WebClient();
            return webClient.DownloadData(imageUrl);
        }

    }
}
