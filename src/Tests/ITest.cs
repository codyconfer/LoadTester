using LoadTester.Settings;

namespace LoadTester.Tests;

public interface ITest : IDisposable
{
    ITest Setup();
    ITest Run();
}
