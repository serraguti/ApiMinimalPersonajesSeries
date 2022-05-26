using ApiMinimalPersonajesSeries.Data;
using ApiMinimalPersonajesSeries.Models;
using ApiMinimalPersonajesSeries.Repositories;
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

var app = builder.Build();

//HABILITAMOS LOS SERVICIOS COMO EN STARTUP

if (app.Environment.IsDevelopment())
{

}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

//METODOS PARA PERSONAJES
app.MapGet("/personajes", (RepositorySeriesPersonajes repo) =>
{
    return repo.GetPersonajes();
});

//FIND(id)
app.MapGet("/personajes/find/{id}", (int id, 
    RepositorySeriesPersonajes repo) =>
{
    return repo.FindPersonaje(id);
});

app.MapPost("/personajes/post", async (Personaje personaje,
    RepositorySeriesPersonajes repo) =>
{
    await repo.AddPersonajeAsync(personaje.Nombre, personaje.Imagen
        , personaje.IdSerie, personaje.Usuario, personaje.Password);
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

