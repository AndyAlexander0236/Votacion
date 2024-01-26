using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Votacion.Models.Entidades
{
    public class Eleccion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEleccion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Descripcion { get; set; }
        public string Cargo { get; set; }

        [Column(TypeName = "decimal (18,2)")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }

       
    }
}
