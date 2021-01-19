using System;
using System.Text.Json.Serialization;

namespace Hadouken.Modules.UrbanDictionary
{
    internal class UrbanDictionaryResponseDto
    {
        [JsonPropertyName("list")]
        public UrabnDictionaryDefinitionDto[] List { get; set; }
    }

    internal class UrabnDictionaryDefinitionDto
    {
        [JsonPropertyName("defid")]
        public long DefinitionId { get; set; }

        [JsonPropertyName("definition")]
        public string Definition { get; set; }

        [JsonPropertyName("permalink")]
        public string Permalink { get; set; }

        [JsonPropertyName("thumbs_up")]
        public int ThumbsUp { get; set; }

        [JsonPropertyName("thumbs_down")]
        public int ThumbsDown { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("word")]
        public string Word { get; set; }

        [JsonPropertyName("written_on")]
        public DateTime Created { get; set; }

        [JsonPropertyName("example")]
        public string Example { get; set; }
    }
}