using Domain.Artist;
using MongoDB.Driver;

namespace Api.Queries;

public static class ArtistsQueries
{
    internal static async Task<IResult> GetAll(IMongoClient client)
    {
        var db = client.GetDatabase("Waartist");
        
        var artists = await db.GetCollection<Artist>("Artists").Find(artist => true).ToListAsync();

        return Results.Ok(artists);
    }
    
    internal static async Task<IResult> GetById(IMongoClient client, string id)
    {
        var db = client.GetDatabase("Waartist");
        
        var artist = await db.GetCollection<Artist>("Artists").Find(artist => artist.Id == id).FirstOrDefaultAsync();

        return artist is not null 
            ? Results.Ok(artist)
            : Results.NotFound();
    }
}