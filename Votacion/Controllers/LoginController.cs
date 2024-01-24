using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Votacion.Models.Entidades;
using Votacion.Models;
using Votacion.Services;

namespace Votacion.Controllers
{
    public class LoginController : Controller
    {
         private readonly IServicioUsuario _ServicioUsuario;
        private readonly IServicioImagen _ServicioImagen;
        private readonly LibreriaContext _context;


        public LoginController(IServicioUsuario ServicioUsuario,
            IServicioImagen ServicioImagen, LibreriaContext context)
        {
            _ServicioUsuario = ServicioUsuario;
            _ServicioImagen = ServicioImagen;
            _context = context;
        }

		public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(Usuario usuario, IFormFile Imagen)

        {
            // Validar si no se ha seleccionado una imagen
            if (Imagen == null || Imagen.Length == 0)
            {
                ViewData["Mensaje"] = "Es necesario subir una foto.";
                return View();
            }

            // Validar si el usuario ya existe
            Usuario usuarioExistente = await _ServicioUsuario.GetUsuarioPorCorreo(usuario.CorreoUsuario);

            if (usuarioExistente != null)
            {
                ViewData["Mensaje"] = "El usuario ya existe. Por favor, elija otro correo electrónico.";
                return View();
            }

            Stream image = Imagen.OpenReadStream();
            string urlImagen = await _ServicioImagen.SubirImagen(image, Imagen.FileName);

            usuario.ClaveUsuario = Utilitarios.EncriptarClave(usuario.ClaveUsuario);
            usuario.URLFotoPerfil = urlImagen;



            Usuario usuarioCreado = await _ServicioUsuario.SaveUsuario(usuario);

            if (usuarioCreado.IdUsuario > 0)
            {
                // Incluye los datos del nuevo usuario en la redirección
                return RedirectToAction("IniciarSesion", "Login", new
                {
                    DocumentoIdentidad = usuarioCreado.DocumentoIdentidad,
                    NombreUsuario = usuarioCreado.NombreUsuario,
                    ApellidoUsuario = usuarioCreado.ApellidoUsuario,
                    CorreoUsuario = usuarioCreado.CorreoUsuario,
                    ClaveUsuario = usuarioCreado.ClaveUsuario
                });
            }

            ViewData["Mensaje"] = "No se pudo crear el usuario";
            return View();
        }

    
        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IniciarSesion(string correo, string clave, string documentoIdentidad, string nombreUsuario, string apellidoUsuario)
        {
            string DocumentoIdentidad = User.FindFirst("DocumentoIdentidad")?.Value;
            string NombreUsuario = User.FindFirst("NombreUsuario")?.Value;
            string ApellidoUsuario = User.FindFirst("ApellidoUsuario")?.Value;
            string CorreoUsuario = User.FindFirst("CorreoUsuario")?.Value;
            string ClaveUsuario = User.FindFirst("ClaveUsuario")?.Value;


            return RedirectToAction("Index", "Home");

        }
    }
    
}
    