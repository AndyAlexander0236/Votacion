using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Votacion.Models;
using Votacion.Models.Entidades;

namespace Votacion.Controllers
{
	public class UsuarioController : Controller
	{
		private readonly LibreriaContext _context;

		public UsuarioController(LibreriaContext context)
		{
			_context = context;
		}


		public async Task<IActionResult> ListadoUsuario()
		{
			return View(await _context.Usuarios.ToListAsync());
		}

		public IActionResult Crear()
		{

			return View();
		}

		[HttpPost]

		public async Task<IActionResult> Crear(Usuario usuario)
		{

			if (ModelState.IsValid)
			{

				_context.Add(usuario);
				await _context.SaveChangesAsync();
				TempData["Alert Message"] = "Usuario Creado exitosamente";
				return RedirectToAction("ListadoUsuario");

			}

			else
			{
				ModelState.AddModelError(String.Empty, "Ha Ocurrido Un Error");
			}

			return View();

		}

		[HttpGet]
		public async Task<IActionResult> Editar(int? id)
		{
			if (id == null || _context.Usuarios == null)
			{
				return NotFound();
			}

			var usuario = await _context.Usuarios.FindAsync(id);
			if (usuario == null)
			{
				return NotFound();
			}
			return View(usuario);
		}

		[HttpPost]
		public async Task<IActionResult> Editar(int id, Usuario usuario)
		{
			if (id != usuario.IdUsuario)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(usuario);
					await _context.SaveChangesAsync();
					TempData["AlertMessage"] = "Usuario actualizado " +
						"exitosamente!!!";
					return RedirectToAction("ListadoUsuario");
				}
				catch (Exception ex)
				{

					ModelState.AddModelError(ex.Message, "Ocurrio un error " +
						"al actualizar");
				}
			}
			return View(usuario);
		}

		public async Task<IActionResult> Eliminar(int? id)
		{
			if (id == null || _context.Usuarios == null)
			{
				return View();
			}

			var usuario = await _context.Usuarios
				.FirstOrDefaultAsync(m => m.IdUsuario == id);

			if (usuario == null)
			{
				return NotFound();
			}

			try
			{
				_context.Usuarios.Remove(usuario);
				await _context.SaveChangesAsync();
				TempData["AlertMessage"] = "Usuario eliminado exitosamente!!";
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(ex.Message, "Ocurrio un error, no se pudo eliminar el registro");

			}

			return RedirectToAction(nameof(ListadoUsuario));

		}

		[HttpPost]
		public async Task<IActionResult> CambiarEstado(int id)
		{
			var usuario = await _context.Usuarios.FindAsync(id);

			if (usuario == null)
			{
				return NotFound();
			}

			try
			{
				usuario.Activo = !usuario.Activo; // Cambiar el estado activo/desactivo
				_context.Update(usuario);
				await _context.SaveChangesAsync();
				TempData["AlertMessage"] = $"Estado del Usuario {usuario.IdUsuario} " +
					$"cambiado exitosamente a {(usuario.Activo ? "Activo" : "Inactivo")}.";
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(ex.Message, "Ocurrió un error al cambiar el estado del candidato");
			}

			return RedirectToAction(nameof(ListadoUsuario));
		}




	}
}
