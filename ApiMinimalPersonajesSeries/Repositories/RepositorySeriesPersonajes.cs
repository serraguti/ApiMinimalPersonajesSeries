using ApiMinimalPersonajesSeries.Data;
using ApiMinimalPersonajesSeries.Models;

namespace ApiMinimalPersonajesSeries.Repositories
{
    public class RepositorySeriesPersonajes
    {
        private SeriesPersonajesContext context;

        public RepositorySeriesPersonajes(SeriesPersonajesContext context)
        {
            this.context = context;
        }

        public List<Personaje> GetPersonajes()
        {
            return this.context.Personajes.ToList();
        }

        public Personaje FindPersonaje(int id)
        {
            return this.context.Personajes.SingleOrDefault(x => x.IdPersonaje == id);
        }

        private int GetMaxIdPersonaje()
        {
            if (this.context.Personajes.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Personajes.Max(x => x.IdPersonaje) + 1;
            }
        }
        
        public async Task AddPersonajeAsync(string nombre, string imagen
            , int idserie, string usuario, string password)
        {
            Personaje personaje = new Personaje()
            {
                IdPersonaje = this.GetMaxIdPersonaje(),
                Nombre = nombre,
                Imagen = imagen,
                IdSerie = idserie,
                Usuario = usuario, 
                Password = password
            };
            await this.context.Personajes.AddAsync(personaje);
            await this.context.SaveChangesAsync();
        }

        public List<Personaje> GetPersonajesSerie(int idSerie)
        {
            return this.context.Personajes.Where(x => x.IdSerie == idSerie).ToList();
        }

        public List<Serie> GetSeries()
        {
            return this.context.Series.ToList();
        }

        private int GetMaxIdSerie()
        {
            if (this.context.Series.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Series.Max(x => x.IdSerie) + 1;
            }
        }

        public async Task AddSerieAsync(string nombre, string imagen
            , double puntuacion, int anyo)
        {
            Serie serie = new Serie
            {
                IdSerie = this.GetMaxIdSerie(),
                Nombre = nombre,
                Imagen = imagen,
                Puntuacion = puntuacion,
                Anyo = anyo
            };
            await this.context.Series.AddAsync(serie);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateSerieAsync
            (int idserie, string nombre, string imagen
            , double puntuacion, int anyo)
        {
            Serie serie = this.context.Series.SingleOrDefault(x => x.IdSerie == idserie);
            serie.Nombre = nombre;
            serie.Imagen = imagen;
            serie.Puntuacion = puntuacion;
            serie.Anyo = anyo;
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteSerieAsync
            (int idserie)
        {
            Serie serie = this.context.Series.SingleOrDefault(x => x.IdSerie == idserie);
            this.context.Series.Remove(serie);
            await this.context.SaveChangesAsync();
        }
    }
}
