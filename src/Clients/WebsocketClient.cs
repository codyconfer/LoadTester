using LoadTester.Settings;
using LoadTester.IO;
using System.Net.WebSockets;
using System.Reactive.Subjects;
using System.Text;

namespace LoadTester.Clients;

public class WebsocketClient : IClient
{
    private readonly string _host;
    private readonly string _testType = string.Empty;
    private Subject<(RequestConfig request, StringContent payload)> _pusherMan;
    private ClientWebSocket _clientWebSocket;
    private bool _keepConnection = false;

    public WebsocketClient(TestConfig config, Subject<(RequestConfig request, StringContent payload)> subject)
    {
        _host = config.Host ?? throw new Exception("Host null in test config");
        _pusherMan = subject;
        _pusherMan.Subscribe(onNext: async data => await Invoke(data.request, data.payload));
        _testType = config.TestType ?? TestTypes.FIRE;
        _clientWebSocket = new ClientWebSocket();
        foreach (var header in config.Headers)
            if (!string.IsNullOrWhiteSpace(header.Key))
                _clientWebSocket.Options.SetRequestHeader($"{header.Key}", $"{header.Value}");
    }

    public async Task Invoke(RequestConfig request, StringContent payload)
    {
        if (_clientWebSocket.State != WebSocketState.Open)
        {
            Uri uri = new ($"{_host}{request.Url}");
            await _clientWebSocket.ConnectAsync(uri, CancellationToken.None);
        }
        var message = await payload.ReadAsStringAsync();
        await _clientWebSocket.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, CancellationToken.None);
        switch (_testType)
        {
            case TestTypes.FIRE:
                await OnFire(request, payload);
                break;
            case TestTypes.LISTEN:
                await OnListen(request, payload);
                break;
            default:
                throw new Exception("Invalid Test Type");
        }
    }

    private async Task OnFire(RequestConfig request, StringContent payload)
    {
        using var ms = new MemoryStream();
        WebSocketReceiveResult result;
        ArraySegment<byte> buffer = new(new byte[2048]);
        do
        {
            result = await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
            if (buffer.Array == null) throw new Exception("Websocket buffer null");
            ms.Write(buffer.Array, buffer.Offset, result.Count);
        } while (!result.EndOfMessage);
        if (result.MessageType == WebSocketMessageType.Close) 
        {
            _keepConnection = false;
            await _clientWebSocket.CloseAsync(result.CloseStatus ?? WebSocketCloseStatus.InternalServerError, result.CloseStatusDescription, CancellationToken.None);
        }
        ms.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(ms, Encoding.UTF8);
        Output.WriteInfoLine(await reader.ReadToEndAsync());
    }

    private async Task OnListen(RequestConfig request, StringContent payload)
    {
        _keepConnection = true;
        do await OnFire(request, payload);
        while (_keepConnection);
    }
    
    public void Dispose()
    {
        _keepConnection = false;
        Task.Run(async () => 
        {
            if (_clientWebSocket.State != WebSocketState.Closed) 
                await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "IClient Dispose", CancellationToken.None);
            _clientWebSocket.Dispose();
        });
    }
}
