using Domain.Artist;

namespace Infrastructure;

public interface IArtistRepository
{
    Task CreateAsync(Artist artist);
    Task<Artist> GetByEmailAsync(string email);
}