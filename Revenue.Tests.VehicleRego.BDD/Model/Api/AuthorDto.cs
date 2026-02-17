using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Revenue.Tests.BDD.Model.Api
{
    public class AuthorDto
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("bio")]
        public TextValueDto? Bio { get; set; }

        [JsonPropertyName("personal_name")]
        public string? PersonalName { get; set; }

        [JsonPropertyName("death_date")]
        public string? DeathDate { get; set; }

        [JsonPropertyName("alternate_names")]
        public List<string>? AlternateNames { get; set; }

        [JsonPropertyName("birth_date")]
        public string? BirthDate { get; set; }

        [JsonPropertyName("type")]
        public KeyDto? Type { get; set; }

        [JsonPropertyName("remote_ids")]
        public RemoteIdsDto? RemoteIds { get; set; }

        [JsonPropertyName("photos")]
        public List<int>? Photos { get; set; }

        [JsonPropertyName("key")]
        public string? Key { get; set; }

        [JsonPropertyName("latest_revision")]
        public int? LatestRevision { get; set; }

        [JsonPropertyName("revision")]
        public int? Revision { get; set; }

        [JsonPropertyName("created")]
        public TextValueDto? Created { get; set; }

        [JsonPropertyName("last_modified")]
        public TextValueDto? LastModified { get; set; }
    }

    public class TextValueDto
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }

    public class KeyDto
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }
    }

    public class RemoteIdsDto
    {
        [JsonPropertyName("wikidata")]
        public string? Wikidata { get; set; }

        [JsonPropertyName("isni")]
        public string? Isni { get; set; }

        [JsonPropertyName("viaf")]
        public string? Viaf { get; set; }

        [JsonPropertyName("lc_naf")]
        public string? LcNaf { get; set; }
    }
}
