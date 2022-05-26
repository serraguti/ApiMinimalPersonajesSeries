using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMinimalPersonajesSeries.Models
{
    [Table("PERSONAJESLAB")]
    public class Personaje
    {
        [Key]
        [Column("IDPERSONAJE")]
        public int IdPersonaje { get; set; }
        [Column("PERSONAJE")]
        public string Nombre { get; set; }
        [Column("IDSERIE")]
        public int IdSerie { get; set; }
        [Column("IMAGEN")]
        public string Imagen { get; set; }
        [Column("USUARIO")]
        public string Usuario { get; set; }
        [Column("PASSWORD")]
        public string Password { get; set; }
    }
}
