using LoadTester.IO;
using LoadTester.Settings;

namespace LoadTester.Tests;

public class Listen : Test
{
    public Listen(TestConfig testConfig) :
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
        for (var j = 0; j < PusherMen.Length; j++)
            PusherMen[j].OnNext((request: TestConfig.Requests[j], payload: PayloadStore[j]));
        Thread.Sleep(250);
        Console.WriteLine("Press any key to end...");
        Console.ReadKey();
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
        Output.WriteSuccessLine($"{timeSpan.Minutes} minutes {timeSpan.Seconds} seconds");

        base.Dispose();
    }
}
