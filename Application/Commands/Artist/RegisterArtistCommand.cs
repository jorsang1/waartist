using Infrastructure;
using Domain.Artist;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Artist;

public class RegisterArtistCommand : IRequest<Domain.Artist.Artist>
{
    public string Name { get; set; }
    public string Genre { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterArtistCommandHandler : IRequestHandler<RegisterArtistCommand, Domain.Artist.Artist>
{
    public readonly IArtistRepository _artistRepository;
    public readonly IPasswordHasher<Domain.Artist.Artist> _passwordHasher;
    
    public async Task<Domain.Artist.Artist> Handle(RegisterArtistCommand request, CancellationToken cancellationToken)
    {
        var artist = new Domain.Artist.Artist
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name,
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(null, request.Password)
        };

        await _artistRepository.CreateAsync(artist);

        return artist;
    }
}