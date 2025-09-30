namespace BoredGames.API.Extensions;

public static class HostBuilderExtensions
{
    public static void SetupOrleansClient(this IHostBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.UseOrleansClient(clientBuilder =>
            clientBuilder.UseLocalhostClustering());
    }
}