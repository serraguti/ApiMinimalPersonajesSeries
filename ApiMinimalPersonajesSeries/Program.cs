using ApiMinimalPersonajesSeries.Authentication;
using ApiMinimalPersonajesSeries.Data;
using ApiMinimalPersonajesSeries.Models;
using ApiMinimalPersonajesSeries.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//INYECTAMOS LOS SERVICIOS TAL Y COMO HACIAMOS EN STARTUP
builder.Services.AddTransient<RepositorySeriesPersonajes>();
string connectionString =
    builder.Configuration.GetConnectionString("AzureSql");
builder.Services.AddDbContext<SeriesPersonajesContext>
    (options => options.UseSqlServer(connectionString));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//HABILITAMOS LA SEGURIDAD
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
    ("BasicAuthentication", null);
builder.Services.AddAuthorization();
var app = builder.Build();

//HABILITAMOS LOS SERVICIOS COMO EN STARTUP
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiMiminimaPersonajaes v1");
    c.RoutePrefix = "";
});

//AÑADIMOS LA AUTORIZACION A NUESTRO SERVIDOR
//EL ORDEN IMPORTA
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

//CADA METODO QUE DESEEMOS PROTEGER SE IMPLEMENTA CON [Authorize]
//METODOS PARA PERSONAJES
app.MapGet("/personajes", [Authorize] (RepositorySeriesPersonajes repo) =>
{
    return repo.GetPersonajes();
});

//FIND(id)
app.MapGet("/personajes/find/{id}", (int id, 
    RepositorySeriesPersonajes repo) =>
{
    return repo.FindPersonaje(id);
});

app.MapPost("/personajes/post", [Authorize] async (Personaje personaje,
    RepositorySeriesPersonajes repo) =>
{
    await repo.AddPersonajeAsync(personaje.Nombre, personaje.Imagen
        , personaje.IdSerie, personaje.UserName, personaje.Password);
    return Results.Ok();
});

app.MapGet("/personajes/serie/{idserie}", (int idserie,
    RepositorySeriesPersonajes repo) =>
{
    return repo.GetPersonajesSerie(idserie);
});

//METODOS PARA LAS SERIES
app.MapGet("/series", (RepositorySeriesPersonajes repo) =>
{
    return repo.GetSeries();
});

app.MapPost("/series/post", async (Serie serie
    , RepositorySeriesPersonajes repo) =>
{
    await repo.AddSerieAsync(serie.Nombre, serie.Imagen, serie.Puntuacion
        , serie.Anyo);
    return Results.Ok();
});

app.MapDelete("/series/delete/{id}", async (int id,
    RepositorySeriesPersonajes repo) =>
{
    await repo.DeleteSerieAsync(id);
    return Results.Ok;
});

app.MapPut("/series/put/{id}", async (int id
    , Serie serie
    , RepositorySeriesPersonajes repo) =>
{
    await repo.UpdateSerieAsync(id, serie.Nombre, serie.Imagen
        , serie.Puntuacion, serie.Anyo);
    return Results.Ok();
});

app.Run();

