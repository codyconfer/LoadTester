using System.Text.Json;

namespace LoadTester.IO;

public static class Input
{
    public static async Task<T> ReadJsonFileAsync<T>(string filePath)
        where T : new()
    {
        await using FileStream stream = File.OpenRead(filePath);
        return await JsonSerializer.DeserializeAsync<T>(stream) ?? new();
    }
}
