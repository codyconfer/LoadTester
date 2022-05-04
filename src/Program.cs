using LoadTester.IO;
using LoadTester.Settings;
using LoadTester.Tests;

Output.WriteSpace();
Output.WriteInfoLine("Loading config...");
var testConfig = await Input.ReadJsonFileAsync<TestConfig>(@"./test.config.json") ?? throw new Exception("Error loading test.config.json");

using ITest test = testConfig.TestType switch
{
    TestTypes.FIRE => new Fire(testConfig),
    TestTypes.LISTEN => new Listen(testConfig),
    _ => throw new Exception("Invalid Test Type"),
};

test.Setup().Run();
