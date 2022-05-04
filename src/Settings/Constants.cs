using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadTester.Settings;

public class TestTypes
{
    public const string FIRE = "FIRE";
    public const string LISTEN = "LISTEN";
}

public class ClientTypes
{
    public const string REST = "REST";
    public const string WEBSOCKET = "WEBSOCKET";
}

public class SupportedHttpMethods
{
    public const string GET = "GET";
    public const string POST = "POST";
    public const string PUT = "PUT";
    public const string DELETE = "DELETE";
    public const string PATCH = "PATCH";
}
