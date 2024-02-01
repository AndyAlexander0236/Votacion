using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Votacion.Models.Entidades
{
	public class Votaciones
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdVotacion { get; set; }

		[Required(ErrorMessage = "El campo {0} es obligatorio")]
        public DateTime FechaRegistro { get; set; }


		// Clave foránea candidato
		public Candidato? Candidato { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un Candidato.")]
		public int IdCandidato { get; set; }

		[NotMapped]
		public IEnumerable<SelectListItem> Candidatos { get; set; }


		// Clave foránea eleccion

		public Eleccion? Eleccion { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una Eleccion.")]
		public int IdEleccion { get; set; }

		[NotMapped]
		public IEnumerable<SelectListItem> Elecciones { get; set; }


		// Clave foránea votante

		public Votante? Votante { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una Eleccion.")]
		public int IdVotante { get; set; }

		[NotMapped]
		public IEnumerable<SelectListItem> Votantes { get; set; }




		//// Nuevas propiedades para el candidato
		//[NotMapped]
		//public string FotoCandidato { get; set; }

		//// Nuevas propiedades para la elección
		//[NotMapped]
		//public string DescripcionEleccion { get; set; }
		//[NotMapped]
		//public bool EstadoEleccion { get; set; }


	}
}
