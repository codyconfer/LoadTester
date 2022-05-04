using LoadTester.Settings;

namespace LoadTester.Clients;

public interface IClient : IDisposable
{
    public Task Invoke(RequestConfig request, StringContent payload);
}
