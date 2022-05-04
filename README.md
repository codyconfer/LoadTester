# LoadTester

## Getting Started

> Must have SDK for .NET 6 or newer installed 

1) `cp ./sample.test.config.json ./test.config.json`
2) Populate config values
3) `dotnet build && dotnet run`

## Config

```json
{
  "ClientType": "REST | WEBSOCKET",
  "TestType": "FIRE | LISTEN",
  "Clients": 100,
  "Iterations": 100,
  "Host": "http://localhost:8000",
  "Headers": [
    { "taco": "bell" }
  ],
  "Requests": [
    {
      "Url": "/",
      "Method": "GET"
    },
    {
      "Url": "/",
      "Method": "POST",
      "Payload": {
        "taco": "bell"
      }
    }
  ]
}
```
