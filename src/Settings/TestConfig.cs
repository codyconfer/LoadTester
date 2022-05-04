namespace LoadTester.Settings;

public class TestConfig
{
    public string? ClientType { get; set; }
    public string? TestType { get; set; }
    public int Clients { get; set; }
    public int Iterations { get; set; }
    public string? Host { get; set; }
    public object? Headers { get; set; }
    public List<RequestConfig> Requests { get; set; } = new List<RequestConfig>();
}
