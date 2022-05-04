using LoadTester.Clients;
using LoadTester.IO;
using LoadTester.Settings;

namespace LoadTester.Tests;

public class Fire : Test
{
    public Fire(TestConfig testConfig) :
        base(testConfig) { }

    public override ITest Setup()
    {
        CreateChannels();
        CreateClients();
        return this;
    }

    public override ITest Run()
    {
        Output.WriteInfoLine("Launching tests...");
        Output.WriteSpace();
        for (var i = 0; i < TestConfig.Iterations; i++)
            for (var j = 0; j < PusherMen.Length; j++)
                PusherMen[j].OnNext((request: TestConfig.Requests[j], payload: PayloadStore[j]));
        return this;
    }

    public override void Dispose()
    {
        var endTime = DateTime.Now;
        var timeSpan = endTime - StartTime;

        Output.WriteSpace();
        Output.WriteInfoLine($"Start time: {StartTime.ToShortDateString()} {StartTime.ToLongTimeString()}");
        Output.WriteInfoLine($"End time: {endTime.ToShortDateString()} {endTime.ToLongTimeString()}");
        Output.WriteSpace();
        Output.WriteSuccessLine($"{Requests} requests in {timeSpan.Minutes} minutes {timeSpan.Seconds} seconds");
        Output.WriteInfoLine($"Average requests per second {Math.Round(Requests / (decimal)timeSpan.TotalSeconds)}");

        base.Dispose();
    }
}
