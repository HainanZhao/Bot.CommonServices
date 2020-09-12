using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Bot.Schema;
using System.Linq;
using System.Text;

namespace Bot.CommonServices.Converters
{
    public class ImageAnalysisHeroCardConverter : IConverter<HeroCard>
    {
        private static string GetCelebertes(ImageAnalysis results)
        {
            var sb = new StringBuilder();
            foreach (var category in results.Categories)
            {
                if (category.Detail?.Celebrities != null)
                {
                    foreach (var celeb in category.Detail.Celebrities)
                    {
                        sb.Append($"{celeb.Name}, ");
                    }
                }
            }

            return sb.ToString();
        }

        private static string GetLandmarks(ImageAnalysis results)
        {
            var sb = new StringBuilder();
            foreach (var category in results.Categories)
            {
                if (category.Detail?.Landmarks != null)
                {
                    foreach (var landmark in category.Detail.Landmarks)
                    {
                        sb.Append($"{landmark.Name}, ");
                    }
                }
            }

            return sb.ToString();
        }


        public HeroCard Convert(object original)
        {
            var results = original as ImageAnalysis;
            var sb = new StringBuilder();

            var celebs = GetCelebertes(results);
            if (!string.IsNullOrWhiteSpace(celebs))
            {
                sb.AppendLine("Celebrities: " + celebs);
            }

            var landMarks = GetLandmarks(results);
            if (!string.IsNullOrWhiteSpace(landMarks))
            {
                sb.AppendLine("Landmarks: " + landMarks);
            }

            sb.Append("Tags:\t");
            foreach (var tag in results.Tags)
            {
                sb.Append($"{tag.Name}, ");
            }
            sb.AppendLine();

            if (results.Brands?.ToList().Count() > 0)
            {
                sb.Append("Brands:\t");
                foreach (var tag in results.Brands)
                {
                    sb.Append($"{tag.Name}, ");
                }
                sb.AppendLine();
            }

            if (results.Objects?.ToList().Count() > 0)
            {
                sb.Append("Objects:\t");
                foreach (var obj in results.Objects)
                {
                    sb.Append($"{obj.ObjectProperty}, ");
                }
                sb.AppendLine();
            }

            var heroCard = new HeroCard
            {
                Title = results.Description.Captions?.ToList().OrderByDescending(x => x.Confidence).FirstOrDefault()?.Text,
                Text = sb.ToString()
            };

            return heroCard;
        }
    }
}
