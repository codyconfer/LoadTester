FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LoadTester.csproj", "./"]
COPY ["test.config.json", "./"]
RUN dotnet restore "LoadTester.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "LoadTester.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LoadTester.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoadTester.dll"]
