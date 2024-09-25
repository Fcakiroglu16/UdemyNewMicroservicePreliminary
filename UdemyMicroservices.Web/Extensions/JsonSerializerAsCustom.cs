using System.Text.Json;

namespace UdemyMicroservices.Web.Extensions;

public static class JsonSerializerAsCustom
{
    private static readonly JsonSerializerOptions DefaultSerializerSettings = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };


    public static T? Deserialize<T>(string text)
    {
        return JsonSerializer.Deserialize<T>(text, DefaultSerializerSettings);
    }
}