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

    
		public Candidato Candidato { get; set; }


		[Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un Candidato.")]
		public int IdCandidato { get; set; }

		[NotMapped]
		public IEnumerable<SelectListItem> Candidatos { get; set; }
	}
}
