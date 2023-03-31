FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BoredGames.Server.API/BoredGames.Server.API.csproj", "BoredGames.Server.API/"]
RUN dotnet restore "BoredGames.Server.API/BoredGames.Server.API.csproj"
COPY . .
WORKDIR "/src/BoredGames.Server.API"
RUN dotnet build "BoredGames.Server.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BoredGames.Server.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BoredGames.Server.API.dll"]
