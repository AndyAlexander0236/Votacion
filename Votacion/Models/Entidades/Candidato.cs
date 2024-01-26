using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Votacion.Models.Entidades
{
    public class Candidato
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCandidato { get; set; }


		[Required(ErrorMessage = "El campo {0} es obligatorio")]

        public string imgCandidato { get; set; }


        public string NombreCandidato { get; set; }

		public string ApellidoCandidato { get; set; }
	

		public string Mensaje { get; set; }

        public DateTime FechaRegistro { get; set; }

        // Claves foráneas
        //public int IdEleccion1 { get; set; }
        [ForeignKey("IdEleccion")]
        public virtual Eleccion? Eleccion { get; set; }

 
    }
}
