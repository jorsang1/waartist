using System.Security.Claims;
using System.Text;
using Api.Endpoints;
using Application;
using Application.Commands.Artist;
using Domain.Artist;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using DependencyInjection = Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<LoginArtistCommand>());

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.Logger.LogInformation("Starting Waartist");

app.Logger.LogInformation("Starting database");
var db = new DatabaseInicialization(app.Services.GetRequiredService<IMongoClient>());
await db.Up();
app.Logger.LogInformation("Database ok");

Endpoints.DefineEndpoints(app);

app.UseAuthentication();
app.UseAuthorization();
app.Run();