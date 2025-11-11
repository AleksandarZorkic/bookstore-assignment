using System.Text.Json;
using System.Text.Json.Serialization;
using BookstoreApplication.DTOs.Comics;
using BookstoreApplication.Services.Interfaces;
using Microsoft.Extensions.Logging;

public class ComicVineClient : IComicsClient
{
    private readonly HttpClient _http;
    private readonly ILogger<ComicVineClient> _log;
    private readonly string _apiKey;
    private readonly JsonSerializerOptions _json = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };
    public ComicVineClient(HttpClient http, IConfiguration cfg, ILogger<ComicVineClient> log)
    {
        _http = http;
        _log = log;
        _apiKey = cfg["ComicVine:ApiKey"] ?? throw new InvalidOperationException("ComicVine:ApiKey missing");
    }

    private async Task<string> GetStringAsync(string url)
    {
        _log.LogInformation("ComicVine GET {Url}", url);
        using var res = await _http.GetAsync(url);
        var txt = await res.Content.ReadAsStringAsync();
        _log.LogInformation("ComicVine response {Status} len={Len} preview={Preview}",
            (int)res.StatusCode, txt?.Length ?? 0, txt is null ? "" : txt[..Math.Min(500, txt.Length)]);
        if (!res.IsSuccessStatusCode)
            throw new HttpRequestException($"ComicVine HTTP {(int)res.StatusCode}: {txt}");
        return txt;
    }

    public async Task<IReadOnlyList<VolumeSearchItemDto>> SearchVolumesAsync(string query, int limit = 20)
    {
        var url =
            $"volumes/?api_key={Uri.EscapeDataString(_apiKey)}&format=json" +
            $"&filter=name:{Uri.EscapeDataString(query)}" +
            $"&field_list=id,name,publisher,start_year&limit={limit}";

        var txt = await GetStringAsync(url);
        var res = JsonSerializer.Deserialize<ApiResponse<Volume>>(txt, _json) ?? new();
        if (res.StatusCode != 1)
            throw new InvalidOperationException($"ComicVine error: {res.Error ?? "Unknown"}");

        return res.Results.Select(v => new VolumeSearchItemDto(
            v.Id, v.Name ?? "(no name)", v.Publisher?.Name, v.StartYear)).ToList();
    }

    public async Task<IReadOnlyList<IssueSearchItemDto>> GetIssuesByVolumeAsync(long volumeExternalId, int limit = 50)
    {
        var url =
            $"issues/?api_key={Uri.EscapeDataString(_apiKey)}&format=json" +
            $"&filter=volume:{volumeExternalId}" +
            $"&field_list=id,name,issue_number,cover_date,description,image&limit={limit}";

        var txt = await GetStringAsync(url);
        var res = JsonSerializer.Deserialize<ApiResponse<Issue>>(txt, _json) ?? new();
        if (res.StatusCode != 1)
            throw new InvalidOperationException($"ComicVine error: {res.Error ?? "Unknown"}");

        return res.Results.Select(i => new IssueSearchItemDto(
            i.Id, i.Name ?? "(no name)", i.IssueNumber,
            DateTime.TryParse(i.CoverDate, out var dt) ? dt : null,
            i.Description, i.Image?.OriginalUrl)).ToList();
    }

    private class ApiResponse<T>
    {
        [JsonPropertyName("status_code")] public int StatusCode { get; set; }
        [JsonPropertyName("error")] public string? Error { get; set; }
        [JsonPropertyName("results")] public List<T> Results { get; set; } = new();
    }
    private class Volume
    {
        [JsonPropertyName("id")] public long Id { get; set; }
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("start_year")] public int? StartYear { get; set; }
        [JsonPropertyName("publisher")] public Pub? Publisher { get; set; }
        public class Pub { [JsonPropertyName("name")] public string? Name { get; set; } }
    }
    private class Issue
    {
        [JsonPropertyName("id")] public long Id { get; set; }
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("issue_number")] public string? IssueNumber { get; set; }
        [JsonPropertyName("cover_date")] public string? CoverDate { get; set; }
        [JsonPropertyName("description")] public string? Description { get; set; }
        [JsonPropertyName("image")] public Img? Image { get; set; }
        public class Img { [JsonPropertyName("original_url")] public string? OriginalUrl { get; set; } }
    }
}
