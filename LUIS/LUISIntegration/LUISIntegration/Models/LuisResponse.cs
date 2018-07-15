using LUISIntegration.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LUISIntegration
{
    public class LuisResponse
    {
        public string Query { get; set; }

        public TopScoringIntent TopScoringIntent { get; set; }

        public List<Entity> Entities { get; set; }

        public SentimentAnalysis SentimentAnalysis { get; set; }
    }
}
