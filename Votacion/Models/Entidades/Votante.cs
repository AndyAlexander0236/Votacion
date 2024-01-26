using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Votacion.Models.Entidades
{
    public class Votante
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdVotante { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string DocumentoIdentidad { get; set; }
        public string NombreVotante { get; set; }
        public string ApellidoVotante { get; set; }

        public DateTime FechaRegistro { get; set; }

        // Claves foráneas
       // public int IdEleccion1 { get; set; }
        [ForeignKey("IdEleccion")]
        public virtual Eleccion? Eleccion { get; set; }

    }
}
