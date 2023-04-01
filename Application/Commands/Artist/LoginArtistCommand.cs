using MediatR;

namespace Application.Commands.Artist;

public class LoginArtistCommand : IRequest<string>
{
    public string Email { get; set; }
    public string Password { get; set; }
}