using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Commands.Artist;

public class LoginArtistCommand : IRequest<string>
{
    public string Email { get; set; }
    public string Password { get; set; }
}


public class LoginArtistCommandHandler : IRequestHandler<LoginArtistCommand, string>
{
    private readonly IArtistRepository _artistRepository;
    private readonly IPasswordHasher<Domain.Artist.Artist> _passwordHasher;
    private readonly IConfiguration _configuration;

    public LoginArtistCommandHandler(IArtistRepository artistRepository, IPasswordHasher<Domain.Artist.Artist> passwordHasher, IConfiguration configuration)
    {
        _artistRepository = artistRepository;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public async Task<string> Handle(LoginArtistCommand request, CancellationToken cancellationToken)
    {
        var artist = await _artistRepository.GetByEmailAsync(request.Email);

        if (artist == null || _passwordHasher.VerifyHashedPassword(null, artist.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
        {
            throw new Exception("Invalid email or password");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, artist.Id),
                new Claim(ClaimTypes.Name, artist.Name),
                new Claim(ClaimTypes.Role, "Artist")
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }
}