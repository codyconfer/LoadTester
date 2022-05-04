using System.Reactive.Subjects;
using LoadTester.IO;
using LoadTester.Settings;

namespace LoadTester.Clients;

public static class StaticClient
{
    public static HttpClient HttpClient = new();
}

public class RestClient : IClient
{
    private HttpClient _client;
    private Subject<(RequestConfig request, StringContent payload)> _pusherMan;
    
    public RestClient(string host, Subject<(RequestConfig request, StringContent payload)> subject)
    {
        _client = StaticClient.HttpClient;
        _client.BaseAddress = new(uriString: host);
        _pusherMan = subject;
        _pusherMan.Subscribe(onNext: async data => await Invoke(data.request, data.payload));
    }

    public async Task Invoke(RequestConfig request, StringContent payload)
    {
        var response = request.Method switch
        {
            SupportedHttpMethods.GET => await _client.GetAsync(request.Url),
            SupportedHttpMethods.POST => await _client.PostAsync(request.Url, payload),
            SupportedHttpMethods.PUT => await _client.PutAsync(request.Url, payload),
            SupportedHttpMethods.PATCH => await _client.PatchAsync(request.Url, payload),
            SupportedHttpMethods.DELETE => await _client.DeleteAsync(request.Url),
            _ => throw new Exception("Invalid HTTP Method")
        };
        var timestamp = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}{Environment.NewLine}";
        if(response.IsSuccessStatusCode)
            Output.WriteSuccessLine($"{timestamp}{await response.Content.ReadAsStringAsync()}");
        else
            Output.WriteErrorLine($"{timestamp}{await response.Content.ReadAsStringAsync()}");
    }
    
    public void Dispose() { }
}
