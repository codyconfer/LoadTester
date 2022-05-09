using LoadTester.Clients;
using LoadTester.IO;
using LoadTester.Settings;
using System.Reactive.Subjects;
using System.Text.Json;

namespace LoadTester.Tests;

public abstract class Test : ITest
{
    protected Test(TestConfig testConfig)
    {
        TestConfig = testConfig;
        Requests = testConfig.Iterations * testConfig.Clients;
    }

    protected readonly TestConfig TestConfig;
    protected readonly int Requests;
    protected DateTime StartTime = DateTime.Now;
    protected Subject<(RequestConfig request, StringContent payload)>[] PusherMen =
        Array.Empty<Subject<(RequestConfig request, StringContent payload)>>();
    protected IClient[] ClientStore =
        Array.Empty<IClient>();
    protected StringContent[] PayloadStore =
        Array.Empty<StringContent>();

    protected virtual void CreateChannels()
    {
        Output.WriteInfoLine("Creating channels...");
        PusherMen = new Subject<(RequestConfig request, StringContent payload)>[TestConfig.Requests?.Count ?? 0];
        PayloadStore = new StringContent[TestConfig.Requests?.Count ?? 0];
        for (var i = 0; i < TestConfig.Requests?.Count; i++)
        {
            PayloadStore[i] = new(JsonSerializer.Serialize(TestConfig.Requests[i]?.Payload ?? "") ?? string.Empty); 
            PusherMen[i] = new();
        }
    }

    protected virtual void CreateClients()
    {
        Output.WriteInfoLine("Creating clients...");
        if (PusherMen == null) throw new Exception("No Clients to build");
        ClientStore = new IClient[TestConfig.Clients];
        var pusherIndex = 0;
        for (var i = 0; i < (TestConfig.Clients); i++)
        {
            var pusherMan = PusherMen[pusherIndex];
            pusherIndex++;
            pusherIndex = pusherIndex < PusherMen.Length ? pusherIndex : 0;
            ClientStore[i] = TestConfig.ClientType switch
            {
                ClientTypes.REST => new RestClient(TestConfig, pusherMan),
                ClientTypes.WEBSOCKET => new WebsocketClient(TestConfig, pusherMan),
                _ => throw new Exception("Invalid Client Type"),
            };
        }
    }

    public abstract ITest Setup();

    public abstract ITest Run();

    public virtual void Dispose()
    {
        foreach (var pusher in PusherMen) pusher?.Dispose();
        foreach (var client in ClientStore) client?.Dispose();
    }
}
