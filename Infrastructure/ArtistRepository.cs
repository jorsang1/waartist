using Domain.Artist;
using MongoDB.Driver;

namespace Infrastructure;

public class ArtistRepository : IArtistRepository
{
    private readonly IMongoCollection<Artist> _artists;

    public ArtistRepository(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("Waartist");
        _artists = database.GetCollection<Artist>("Artists");
    }

    public async Task CreateAsync(Artist artist)
    {
        await _artists.InsertOneAsync(artist);
    }

    public async Task<Artist> GetByEmailAsync(string email)
    {
        return await _artists.Find(a => a.Email == email).FirstOrDefaultAsync();
    }
}