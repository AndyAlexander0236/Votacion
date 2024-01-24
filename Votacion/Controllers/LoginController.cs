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
		public async Task<IActionResult> Registro(Usuario Usuario, IFormFile Imagen)

		{  // Validar si no se ha seleccionado una imagen
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

			Usuario usuarioCreado = await _ServicioUsuario.SaveUsuario(Usuario);

			if (usuarioCreado.IdUsuario > 0)
			{
				return RedirectToAction("IniciarSesion", "Login");
			}

			ViewData["Mensaje"] = "No se pudo crear el usuario";
			return View();
		}


		public IActionResult IniciarSesion()
		{

			return View();
		}
		[HttpPost]
		public async Task<IActionResult> IniciarSesion(string correo, string clave)
		{
			Usuario usuarioEncontrado = await _ServicioUsuario.GetUsuario(correo, Utilitarios.EncriptarClave(clave));

			if (usuarioEncontrado == null)
			{
				ViewData["Mensaje"] = "Usuario no encontrado";
				return View();
			}

			List<Claim> claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Name, usuarioEncontrado.NombreUsuario),
				new Claim("FotoPerfil", usuarioEncontrado.URLFotoPerfil),
			};

			ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			AuthenticationProperties properties = new AuthenticationProperties()
			{
				AllowRefresh = true,
			};

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity),
				properties
				);

			return RedirectToAction("Index", "Home");

		}

		public async Task<IActionResult> CerrarSesion()
		{
			await HttpContext.SignOutAsync();
			return Redirect("IniciarSesion");
		}



	}

}
