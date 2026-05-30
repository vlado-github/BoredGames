using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var aspnetEnvVar =  Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

// Orleans cluster register
var redis = builder.AddRedis("redis");

// Orleans cluster
var orleans = builder.AddOrleans("boredgames-cluster")
    .WithClustering(redis)
    .WithGrainStorage("Default", redis)
    .WithGrainStorage("PubSubStore", redis);
    //.WithReminders(redis);

// Orleans silo
var gameServer = builder.AddProject<Projects.BoredGames_Server_GameServer>("boredgames-server")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", aspnetEnvVar)
    .WithReference(orleans)
    .WaitFor(redis)
    .WithReplicas(3);

// Orleans client
builder.AddProject<Projects.BoredGames_API>("boredgames-client")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", aspnetEnvVar)
    .WithReference(orleans.AsClient())
    .WaitFor(gameServer);

builder.Build().Run();