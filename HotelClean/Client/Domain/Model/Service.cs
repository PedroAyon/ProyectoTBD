using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Client.Domain.Model;
public partial class Service
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("locationId")]
    public int LocationId { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("date")]
    public DateOnly? Date { get; set; }

    [JsonPropertyName("startTime")]
    public TimeOnly? StartTime { get; set; }

    [JsonPropertyName("endingTime")]
    public TimeOnly? EndingTime { get; set; }

    [JsonPropertyName("location")]
    public Location? Location { get; set; }
}