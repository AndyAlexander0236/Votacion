using System;
using System.Collections.Generic;  // Agrega este espacio de nombres
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Votacion.Models.Entidades
{
	public class Candidato
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int IdCandidato { get; set; }

        [Display(Name = "Imagen")]
        public string ImgCandidato { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
	
		public string NombreCandidato { get; set; }
		public string ApellidoCandidato { get; set; }
		public string Mensaje { get; set; }

		public DateTime FechaRegistro { get; set; }

		public Eleccion? Eleccion { get; set; }

		// Clave foránea
		[Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una Eleccion.")]
		public int IdEleccion { get; set; }

		[NotMapped]
		public IEnumerable<SelectListItem> Elecciones { get; set; }
	}
}
