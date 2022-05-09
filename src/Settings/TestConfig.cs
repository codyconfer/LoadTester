using System.Text.Json.Nodes;

namespace LoadTester.Settings;

public class TestConfig
{
    public string? ClientType { get; set; }
    public string? TestType { get; set; }
    public int Clients { get; set; }
    public int Iterations { get; set; }
    public string? Host { get; set; }
    public JsonObject Headers { get; set; } = new();
    public List<RequestConfig> Requests { get; set; } = new();
}
