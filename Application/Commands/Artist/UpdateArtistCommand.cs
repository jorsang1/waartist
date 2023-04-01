using Infrastructure;
using Domain.Artist;
using MediatR;

namespace Application.Commands.Artist;

public class UpdateArtistCommand : IRequest<Domain.Artist.Artist>
{
    public string Name { get; set; }
    public string ShowDescription { get; set; }
    public string Email { get; set; }
}

public class UpdateArtistCommandHandler : IRequestHandler<UpdateArtistCommand, Domain.Artist.Artist>
{
    public readonly IArtistRepository _artistRepository;
    
    public async Task<Domain.Artist.Artist> Handle(UpdateArtistCommand request, CancellationToken cancellationToken)
    {
        var artist = await _artistRepository.GetByEmailAsync(request.Email);
        artist.Name = request.Name;
        artist.ShowDescription = request.ShowDescription;
        await _artistRepository.CreateAsync(artist);

        return artist;
    }
}