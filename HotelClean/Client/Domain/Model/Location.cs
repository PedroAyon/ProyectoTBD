using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Client.Domain.Model;

public partial class Location
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("floor")]
    public int Floor { get; set; }
}