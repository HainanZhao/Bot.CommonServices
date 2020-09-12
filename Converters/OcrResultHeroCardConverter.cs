using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Bot.CommonServices.Converters
{
    public class OcrResultHeroCardConverter : IConverter<OcrResult, HeroCard>
    {
        public HeroCard Convert(OcrResult original)
        {
            var sb = new StringBuilder();
            foreach (var region in original.Regions)
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

            if (string.IsNullOrWhiteSpace(sb.ToString()))
                return null;

            var heroCard = new HeroCard
            {
                Title = "Text in image",
                Text = sb.ToString(),
                Buttons = new List<CardAction>() { new CardAction(ActionTypes.OpenUrl, title: "Google it", value: "https://www.google.com/search?q=" + HttpUtility.UrlEncode(sb.ToString())) }
            };
            return heroCard;
        }
    }
}
