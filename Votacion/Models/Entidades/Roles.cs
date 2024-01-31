using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Votacion.Models.Entidades
{
	 public class Roles
    {
        // Propiedades de la entidad Roles
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRol { get; set; }
        public string Rol { get; set; }
    }
}
