using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Bot.Schema;
using System.Linq;
using System.Text;

namespace Bot.CommonServices.Converters
{
    public class ImageAnalysisHeroCardConverter : IConverter<ImageAnalysis, HeroCard>
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


        public HeroCard Convert(ImageAnalysis original)
        {            
            var sb = new StringBuilder();

            var celebs = GetCelebertes(original);
            if (!string.IsNullOrWhiteSpace(celebs))
            {
                sb.AppendLine("Celebrities: " + celebs);
            }

            var landMarks = GetLandmarks(original);
            if (!string.IsNullOrWhiteSpace(landMarks))
            {
                sb.AppendLine("Landmarks: " + landMarks);
            }

            sb.Append("Tags:\t");
            foreach (var tag in original.Tags)
            {
                sb.Append($"{tag.Name}, ");
            }
            sb.AppendLine();

            if (original.Brands?.ToList().Count() > 0)
            {
                sb.Append("Brands:\t");
                foreach (var tag in original.Brands)
                {
                    sb.Append($"{tag.Name}, ");
                }
                sb.AppendLine();
            }

            if (original.Objects?.ToList().Count() > 0)
            {
                sb.Append("Objects:\t");
                foreach (var obj in original.Objects)
                {
                    sb.Append($"{obj.ObjectProperty}, ");
                }
                sb.AppendLine();
            }

            var heroCard = new HeroCard
            {
                Title = original.Description.Captions?.ToList().OrderByDescending(x => x.Confidence).FirstOrDefault()?.Text,
                Text = sb.ToString()
            };

            return heroCard;
        }
    }
}
