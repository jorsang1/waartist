using Domain.Artist;
using MongoDB.Driver;

namespace Infrastructure;

public class DatabaseInicialization
{
    private readonly IMongoCollection<Artist> _artists;
    private readonly IMongoDatabase _database;

    public DatabaseInicialization(IMongoClient mongoClient)
    {
        _database = mongoClient.GetDatabase("Waartist");
        _artists = _database.GetCollection<Artist>("Artists");
    }

    public async Task Up()
    {
        var artist = await _artists.Find(a => true).Limit(1).FirstOrDefaultAsync();
        if (artist is null)
            InsertDemoArtists();
    }

    public async Task Down()
    {
        await _artists.DeleteManyAsync(artist => true);
    }

    private async Task InsertDemoArtists()
    {
        await _artists.InsertManyAsync(new [] {
            new Artist
            {
                Id = Guid.NewGuid().ToString(),
                Email = "some-email@gmail.com",
                PasswordHash = "some-hashed-password",
                Name = "Fulgencio Garcia",
                ShowDescription = "Making software live"
            },
            new Artist
            {
                Id = Guid.NewGuid().ToString(),
                Email = "any-email@gmail.com",
                PasswordHash = "this is a hashed-password",
                Name = "Fangoria",
                ShowDescription = "Espectáculos de la bruja avería"
            }
        }
        );
    }
}