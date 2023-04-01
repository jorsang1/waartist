using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddDbContext<WebhooksDbContext>(options =>
        //         options.UseNpgsql(configuration.GetConnectionString("PostgresDbConnection")))
        //     .ConfigureHttpJsonOptions(options =>
        //         options.SerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

        // Necessary for working with UTC DateTimes and avoiding DateTime errors
        //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        services.AddSingleton<IMongoClient>(_ => new MongoClient(configuration.GetConnectionString("MongoDb")));

        services.AddScoped<IArtistRepository, ArtistRepository>();
    }
}