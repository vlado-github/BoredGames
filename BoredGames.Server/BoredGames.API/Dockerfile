FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BoredGames.Server/BoredGames.Common/BoredGames.Common.csproj", "BoredGames.Common/"]
COPY ["BoredGames.Server/BoredGames.Server.Domain/BoredGames.Server.Domain.csproj", "BoredGames.Server.Domain/"]
COPY ["BoredGames.Server/BoredGames.Server.GameServer/BoredGames.Server.GameServer.csproj", "BoredGames.Server.GameServer/"]
COPY ["BoredGames.Server/BoredGames.API/BoredGames.API.csproj", "BoredGames.API/"]
RUN dotnet restore "BoredGames.API/BoredGames.API.csproj"
COPY BoredGames.Server/ .
WORKDIR "/src/BoredGames.API"
RUN dotnet build "BoredGames.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BoredGames.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BoredGames.API.dll"]
