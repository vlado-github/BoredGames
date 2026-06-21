var builder = DistributedApplication.CreateBuilder(args);

var aspnetEnvVar =  Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

// Orleans cluster register
var redis = builder.AddRedis("redis");

// Keycloak
var keycloak = builder.AddKeycloak("keycloak", port: 8080)
    .WithRealmImport($"./../../.keycloak/imports/{aspnetEnvVar}");

// Orleans cluster
var orleans = builder.AddOrleans("boredgames-cluster")
    .WithClustering(redis)
    .WithGrainStorage("Default", redis)
    .WithGrainStorage("PubSubStore", redis);

// Orleans silo
var gameServer = builder.AddProject<Projects.BoredGames_Server_GameServer>("boredgames-server")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", aspnetEnvVar)
    .WithReference(orleans)
    .WaitFor(redis)
    .WithReplicas(3);

// Orleans client
var api = builder.AddProject<Projects.BoredGames_API>("boredgames-client")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", aspnetEnvVar)
    .WithReference(orleans.AsClient())
    .WaitFor(gameServer)
    .WaitFor(keycloak);

// Game Portal
builder.AddViteApp("boredgames-portal", "../../BoredGames.Client/BoredGames.Portal")
   .WithEndpoint("http", endpoint =>
   {
       endpoint.Port = 5173; 
   })
   .WithReference(api)
   .WaitFor(api);

builder.Build().Run();