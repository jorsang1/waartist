using System.Security.Claims;
using Api.Queries;
using Application.Commands.Artist;
using MediatR;

namespace Api.Endpoints;

internal static class Endpoints
{
    public static void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/ping", () => Results.Ok("pong!"))
            .WithTags("Health")
            .WithSummary("Ping-pong health check");

        app.MapGet("/", () => "Hello World!");

        app.MapGet("/artists", ArtistsQueries.GetAll);
        
        app.MapPost("/artists", RegisterArtist);

        app.MapGet("/artists/{id}", ArtistsQueries.GetById);

        app.MapPut("/artists/{id}", UpdateArtist).RequireAuthorization();

    }

    private static async Task<IResult> RegisterArtist(string id,
        RegisterArtistCommand command,
        IMediator mediator)
    {
        var artist = await mediator.Send(command);

        return Results.CreatedAtRoute($"artists/{artist.Id}");
    }
    
    private static async Task<IResult> UpdateArtist(string id,
        UpdateArtistCommand command,
        IMediator mediator,
        ClaimsPrincipal user)
    {
        if (!user.Identity.IsAuthenticated)
        {
            return Results.Unauthorized();
        }

        var artistId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (artistId != id)
        {
            return Results.Unauthorized();
        }

        var artist = await mediator.Send(command);

        return Results.NoContent();
    }
}