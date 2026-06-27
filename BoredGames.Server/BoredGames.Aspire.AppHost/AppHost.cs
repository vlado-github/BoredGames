var builder = DistributedApplication.CreateBuilder(args);

var aspnetEnvVar =  Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

// Redis - used as Orleans cluster registrar
var redis = builder.AddRedis("redis");

// Postgres - used as Keycloak database
var postgres = builder.AddPostgres("boredgames-postgres");

// Keycloak
var keycloakDb = postgres.AddDatabase("keycloak");
var keycloak = builder.AddKeycloak("boredgames-keycloak", port: 8080)
    .WithReference(postgres)
    .WaitFor(keycloakDb)
    .WithDataVolume()
    .WithEnvironment("KC_BOOTSTRAP_ADMIN_USERNAME", "admin")
    .WithEnvironment("KC_BOOTSTRAP_ADMIN_PASSWORD", "admin")
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

// API
var api = builder.AddProject<Projects.BoredGames_API>("boredgames-api")
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
