using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Votacion.Models.Entidades
{
    public class Votaciones
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdVotacion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public DateTime FechaRegistro { get; set; }

        // Claves foráneas
        public int IdEleccion1 { get; set; }
        [ForeignKey("IdEleccion")]
        public virtual Eleccion? Eleccion { get; set; }

        public int IdVotante { get; set; }
        [ForeignKey("IdVotante")]
        public virtual Votante? Votante { get; set; }

        public int IdUsuarioR { get; set; }
        [ForeignKey("IdUsuario")]
        public virtual Usuario? Usuario { get; set; }

        public int IdCandidato { get; set; }
        [ForeignKey("IdCandidato")]
        public virtual Candidato? Candidato { get; set; }
    }
}
