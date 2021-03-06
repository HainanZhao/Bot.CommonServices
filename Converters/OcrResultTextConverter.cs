﻿using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Text;

namespace Bot.CommonServices.Converters
{    
    public class OcrResultTextConverter : IConverter<OcrResult, string>
    {
        public static string GetTextFromOcrResult(OcrResult result)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var region in result.Regions)
            {
                foreach (var line in region.Lines)
                {
                    foreach (var word in line.Words)
                    {
                        sb.Append(word.Text);
                        sb.Append(" ");
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
        public string Convert(OcrResult original)
        {            
            return GetTextFromOcrResult(original);
        }
    }
}
