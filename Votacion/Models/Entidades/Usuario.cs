using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Votacion.Models.Entidades
{
    public class Usuario
    {
        // Propiedades de la entidad Usuario
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }
        public string DocumentoIdentidad { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidoUsuario { get; set; }
        public string CorreoUsuario { get; set; }
        public string ClaveUsuario { get; set; }
        public string URLFotoPerfil { get; set; }

		public Roles? Roles { get; set; }

        // Clave foránea
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un Rol.")]
        public int IdRol { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> Rol { get; set; }
    }
}
