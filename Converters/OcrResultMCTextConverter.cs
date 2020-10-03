using Bot.CommonServices.Converters;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Bot.CommonServices.Converters
{
    public class OcrResultMCTextConverter : IConverter<OcrResult, string>
    {
        private static readonly string[] NoiseKeywords = 
        {   "clinic", "pte", "ltd", "tel", "fax", 
            "ref", "reg", "no", "number", "date", "license",
            "hospital", "department", "branch", "division", "office", "station",
            "location", "blk", "block", "street", "road", "avenue", "center", "centre", "central", "singapore", "australia",
            "dr", "doctor", "doctors", "doctor's", "officer", "examiner", "practitioner", "physician", "chief", "md", "m.d.",  "mcr",
            "particulars", "sign", "signature", "thumb", "stamp", "issuing", "issued",
            "yours", "faithfully", "sincerely", "regards",
            "notes", "remarks", "disclaimer", "court", "judiciary", "proceedings"
        };

        private static readonly string[] NoiseKeywords2 =
        {   
            "medical certificate", "certificate no", "certificatenumber",
            "refno", "reference number", "ref number", "reg no", "reg number", "registration number", 
            "account no", "acct no", "account number",
            "not valid", "does not",
            "medical center", "family clinic", "pte ltd",
            "court attendance", "judiciary proceedings", 
            "doctors signature", "issuing doctor",  "doctor name", "mcr no",
            "date printed", "date issued", "date visited", "visit date"
        };

        private static readonly string[] NoiseKeywords3 =
        {
            "date of visit", "is not valid", "absence from court", "signature of doctor"
        };

        private static readonly string[] BodyKeywords = 
        {            
            "leave", "granted", 
            "whom", "may", "concern",            
            "certify", "patient", "unfit", "work", "duty", "period", "days", "from", "to", "inclusive"
        };

        private static readonly string[] BodyKeywords2 =
        {
            "to certify", "is unfit", "unfit for", "for duty", "for work", "sick leave", "hospitalization leave", "hospitalisation leave"
        };

        private static readonly string[] BodyKeywords3 =
        {
            "unfit for work", "unfit for duty", "a period of", "type of leave"
        };

        private double WeightScore(int score)
        {
            if (score > 0)            
                return 0.5;            
            else if (score < 0)
                return -0.5;

            return 0;
        }

        private string GetRegionText(OcrRegion region)
        {
            int regionScore = 0;
            var rawScores = new List<int>();            
            var lines = new List<string>();

            var sb = new StringBuilder();
            foreach (var line in region.Lines)
            {
                var lineScore = 0;
                sb.Clear();

                for (int i = 0; i < line.Words.Count; i++)
                {                    
                    sb.Append(CleanupWord(line.Words[i].Text));
                    sb.Append(" ");                    
                }

                //Calculate line score after clean up the noise chars
                var cleanWords = sb.ToString().Split(' ');
                for (int i = 0; i < cleanWords.Length; i++)
                {                    
                    lineScore += GetWordScore(cleanWords[i]);
                    if (i + 1 < cleanWords.Length)
                    {
                        lineScore += GetWordScore2(cleanWords[i], cleanWords[i + 1]);
                    }

                    if (i + 2 < cleanWords.Length)
                    {
                        lineScore += GetWordScore3(cleanWords[i], cleanWords[i + 1], cleanWords[i + 2]);
                    }
                }
                
                lines.Add(sb.ToString());
                rawScores.Add(lineScore);
                regionScore += lineScore;
            }

            sb.Clear();

            //Process lines based on region, and surrounding lines
            for (int i = 0; i < lines.Count; i++) 
            {
                var lineScore = WeightScore(regionScore) + rawScores[i];
                
                if (i >= 1)
                {
                    lineScore += WeightScore(rawScores[i - 1]);
                }

                if (i + 1 < lines.Count)
                {
                    lineScore += WeightScore(rawScores[i + 1]); 
                }

                if (lineScore > 0)
                {
                    sb.Append(lines[i]);
                }
            }
            
            return sb.ToString();
        }

        private string CleanupWord(string input)
        {
            return Regex.Replace(input.ToLower(), @"[^A-Za-z0-9\\\/]", "").Trim();
        }

        public bool IsRegistrationNumber(string text)
        {
            return Regex.IsMatch(text, @"^[A-Za-z]+[\\\/]*\d+[A-Za-z]*$|^[A-Za-z]*\d+[\\\/]*[A-Za-z]*$");
        }

        private int GetWordScore(string word)
        {
            int score = 0;
            word = CleanupWord(word);
           
            if (NoiseKeywords.Contains(word) || IsRegistrationNumber(word))
            {
                score--;
            }

            if (BodyKeywords.Contains(word))
            {
                score++;
            }
            
            return score;
        }

        private int GetWordScore2(string word, string word2)
        {
            int score = 0;
            word = word + " " + word2;

            if (NoiseKeywords2.Contains(word))
            {
                score--;
            }

            if (BodyKeywords2.Contains(word))
            {
                score++;
            }

            return score;
        }

        private int GetWordScore3(string word, string word2, string word3)
        {
            int score = 0;
            word = word + " " + word2 + " " + word3;

            if (NoiseKeywords3.Contains(word))
            {
                score--;
            }

            if (BodyKeywords3.Contains(word))
            {
                score++;
            }

            return score;
        }


        private string GetTextFromOcrResult(OcrResult result)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var region in result.Regions)
            {
                var raw = GetRegionText(region);                
                sb.Append(raw);
            }
            return sb.ToString();
        }
        public string Convert(OcrResult original)
        {
            return GetTextFromOcrResult(original);
        }

    }
}
