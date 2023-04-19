FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BoredGames.Client.CLI/BoredGames.Client.CLI.csproj", "BoredGames.Client.CLI/"]
RUN dotnet restore "BoredGames.Client.CLI/BoredGames.Client.CLI.csproj"
COPY . .
WORKDIR "/src/BoredGames.Client.CLI"
RUN dotnet build "BoredGames.Client.CLI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BoredGames.Client.CLI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BoredGames.Client.CLI.dll"]
