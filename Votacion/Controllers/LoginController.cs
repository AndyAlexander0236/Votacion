using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Votacion.Models;
using Votacion.Models.Entidades;
using Votacion.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Votacion.Controllers
{
    public class LoginController : Controller
    {
        private readonly IServicioUsuario _ServicioUsuario;
        private readonly IServicioImagen _ServicioImagen;
        private readonly IServicioLista _ServicioLista;
        private readonly LibreriaContext _context;


        public LoginController(IServicioUsuario ServicioUsuario,
            IServicioImagen ServicioImagen, IServicioLista ServicioLista, LibreriaContext context)
        {
            _ServicioUsuario = ServicioUsuario;
            _ServicioImagen = ServicioImagen;
            _ServicioLista = ServicioLista;
            _context = context;
        }

        public async Task<IActionResult> Lista()
        {
            return View(await _context.Usuarios
                 .Include(l => l.Rol)
                 .ToListAsync());

        }

        public IActionResult Registro()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(Usuario Usuario, IFormFile Imagen, int IdRol)
        {

            // Validar si no se ha seleccionado una imagen
            if (Imagen == null || Imagen.Length == 0)
            {
                ViewData["Mensaje"] = "Es necesario subir una foto.";
                return View();
            }

            // Validar si el usuario ya existe
            Usuario usuarioExistente = await _ServicioUsuario.GetUsuarioPorCorreo(Usuario.CorreoUsuario);

            if (usuarioExistente != null)
            {
                ViewData["Mensaje"] = "El usuario ya existe. Por favor, elija otro correo electrónico.";
                return View();
            }

            Stream image = Imagen.OpenReadStream();
            string urlImagen = await _ServicioImagen.SubirImagen(image, Imagen.FileName);

            Usuario.ClaveUsuario = Utilitarios.EncriptarClave(Usuario.ClaveUsuario);
            Usuario.URLFotoPerfil = urlImagen;

            // Asignar el rol seleccionado al usuario
            Usuario.IdRol = IdRol;


            // Validar y asignar la lista de roles al modelo
            Usuario.Rol = _context.Roles.Select(e => new SelectListItem
            {
                Value = e.IdRol.ToString(),
                Text = e.Rol
            }).ToList();


            //Siempre se registrara como un Usuario 
            Usuario.IdRol = 2;

            Usuario usuarioCreado = await _ServicioUsuario.SaveUsuario(Usuario);

            if (usuarioCreado.IdUsuario > 0)
            {
                return RedirectToAction("IniciarSesion", "Login");
            }

            ViewData["Mensaje"] = "No se pudo crear el usuario";

            // Si llegas aquí, algo salió mal, vuelve a cargar el formulario con el modelo y las elecciones.
            Usuario.Rol = _context.Roles.Select(e => new SelectListItem
            {
                Value = e.IdRol.ToString(),
                Text = e.Rol
            });

            return View(Usuario);
        }


        public IActionResult IniciarSesion()
        {
            var roles = _context.Roles.Select(e => new SelectListItem
            {
                Value = e.IdRol.ToString(),
                Text = e.Rol
            }).ToList();

            ViewData["Roles"] = roles;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string correo, string clave, int Rol)
        {
            Usuario usuarioEncontrado = await _ServicioUsuario.GetUsuario(correo, Utilitarios.EncriptarClave(clave), Rol);

            if (usuarioEncontrado == null)
            {
                ViewData["Mensaje"] = "Usuario no encontrado";
                return View();
            }

            if (usuarioEncontrado.IdRol != 1 && usuarioEncontrado.IdRol != 2)
            {
                ViewData["Mensaje"] = "Usuario no permitido";
                return View();
            }

            // Validar si el rol seleccionado en el formulario coincide con el rol del usuario
            if (Rol != usuarioEncontrado.IdRol)
            {
                ViewData["Mensaje"] = "Este Tipo de ingreso no es válido para este usuario.";
                return View();
            }

            List<Claim> claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuarioEncontrado.NombreUsuario),
        new Claim("FotoPerfil", usuarioEncontrado.URLFotoPerfil),
        new Claim(ClaimTypes.Role, usuarioEncontrado.IdRol.ToString()),
    };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties
            {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            // Redirige al usuario según su rol
            if (usuarioEncontrado.IdRol == 1)  //1 representa el rol de Administrador
            {
                return RedirectToAction("Index", "Home");
            }
            else if (usuarioEncontrado.IdRol == 2)  //2 representa el rol de Usuario
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Agrega una redirección por defecto si el rol no es reconocido
                return RedirectToAction("Index", "Home");
            }

        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync();
            return Redirect("IniciarSesion");
        }



    }

}