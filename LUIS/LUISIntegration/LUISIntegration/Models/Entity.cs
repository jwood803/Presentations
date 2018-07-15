using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LUISIntegration.Models
{
    public class Entity
    {
        [JsonProperty("entity")]
        public string EntityName { get; set; }

        public string Type { get; set; }

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        public float Score { get; set; }
    }
}
