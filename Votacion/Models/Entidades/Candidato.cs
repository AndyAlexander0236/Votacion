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
        public string NombreCandidato { get; set; }
        public string Mensaje { get; set; }

        [Column(TypeName = "decimal (18,2)")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }

        // Claves foráneas
        //public int IdEleccion1 { get; set; }
        [ForeignKey("IdEleccion")]
        public virtual Eleccion? Eleccion { get; set; }

       // public int IdUsuarioR { get; set; }
        [ForeignKey("IdUsuario")]
        public virtual Usuario? Usuario { get; set; }
    }
}
