using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("cache");

var orleans = builder.AddOrleans("default")
    .WithClustering(redis)
    .WithGrainStorage(redis);

var api = builder.AddProject<Projects.BoredGames_API>("api")
    .WithReference(orleans.AsClient());

builder.Build().Run();