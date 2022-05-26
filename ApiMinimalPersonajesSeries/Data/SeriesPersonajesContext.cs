using ApiMinimalPersonajesSeries.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMinimalPersonajesSeries.Data
{
    public class SeriesPersonajesContext: DbContext
    {
        public SeriesPersonajesContext(DbContextOptions<SeriesPersonajesContext> options)
            : base(options) { }
        public DbSet<Personaje> Personajes { get; set; }
        public DbSet<Serie> Series { get; set; }
    }
}
