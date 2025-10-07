FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 11111
ExPOSE 30000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BoredGames.Server/BoredGames.Common/BoredGames.Common.csproj", "BoredGames.Common/"]
COPY ["BoredGames.Server/BoredGames.Server.Domain/BoredGames.Server.Domain.csproj", "BoredGames.Server.Domain/"]
COPY ["BoredGames.Server/BoredGames.Server.GameServer/BoredGames.Server.GameServer.csproj", "BoredGames.Server.GameServer/"]
RUN dotnet restore "BoredGames.Server.GameServer/BoredGames.Server.GameServer.csproj"
COPY BoredGames.Server/ .
WORKDIR "/src/BoredGames.Server.GameServer"
RUN dotnet build "BoredGames.Server.GameServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BoredGames.Server.GameServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BoredGames.Server.GameServer.dll"]
