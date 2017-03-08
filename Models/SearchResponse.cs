using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace viaSportResourceBot.Models
{
    public class SearchResponse
    {
        [JsonProperty(PropertyName = "webPages")]
        public WebPages WebPages { get; set; }

    }

    public class WebPages
    {
        [JsonProperty(PropertyName = "value")]
        public List<Link> Value { get; set; }

        [JsonProperty(PropertyName = "totalEstimatedMatches")]
        public int TotalEstimatedMatches { get; set; }
    }

    public class Link
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "displayUrl")]
        public string DisplayUrl { get; set; }


        [JsonProperty(PropertyName = "snippet")]
        public string Snippet { get; set; }
    }
}