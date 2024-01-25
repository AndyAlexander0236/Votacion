using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Votacion.Models;
using Votacion.Models.Entidades;

namespace Votacion.Controllers
{
	public class EleccionController : Controller
	{
		private readonly LibreriaContext _context;

		public EleccionController(LibreriaContext context)
		{
			_context = context;
		}


		public async Task<IActionResult> ListadoEleccion()
		{
			return View(await _context.Elecciones.ToListAsync());
		}

		public IActionResult Crear()
		{

			return View();
		}

		[HttpPost]

		public async Task<IActionResult> Crear(Eleccion eleccion)
		{

			if (ModelState.IsValid)
			{

				_context.Add(eleccion);
				await _context.SaveChangesAsync();
				TempData["Alert Message"] = "Eleccion Creado exitosamente";
				return RedirectToAction("ListadoEleccion");

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
			if (id == null || _context.Elecciones == null)
			{
				return NotFound();
			}

			var eleccion = await _context.Elecciones.FindAsync(id);
			if (eleccion == null)
			{
				return NotFound();
			}
			return View(eleccion);
		}

		[HttpPost]
		public async Task<IActionResult> Editar(int id, Eleccion eleccion)
		{
			if (id != eleccion.IdEleccion)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(eleccion);
					await _context.SaveChangesAsync();
					TempData["AlertMessage"] = "Eleccion actualizado " +
						"exitosamente!!!";
					return RedirectToAction("ListadoEleccion");
				}
				catch (Exception ex)
				{

					ModelState.AddModelError(ex.Message, "Ocurrio un error " +
						"al actualizar");
				}
			}
			return View(eleccion);
		}

		

		//EDITAR ESTADO ACTIVO/INACTIVO
		[HttpPost]
		public async Task<IActionResult> CambiarEstado(int id)
		{
			var eleccion = await _context.Elecciones.FindAsync(id);

			if (eleccion == null)
			{
				return NotFound();
			}

			try
			{
				eleccion.Activo = !eleccion.Activo; // Cambiar el estado activo/desactivo
				_context.Update(eleccion);
				await _context.SaveChangesAsync();
				TempData["AlertMessage"] = $"Estado de la eleccion cambiado exitosamente a {(eleccion.Activo ? "Activo" : "Inactivo")}.";
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(ex.Message, "Ocurrió un error al cambiar el estado de la eleccion");
			}

			return RedirectToAction(nameof(ListadoEleccion));
		}



		//EDITAR FECHA DE REGISTRO
		[HttpPost]
		public async Task<IActionResult> CambiarFechaRegistro(int id, DateTime nuevaFechaRegistro)
		{
			var eleccion = await _context.Elecciones.FindAsync(id);

			if (eleccion == null)
			{
				return NotFound();
			}

			try
			{
				eleccion.FechaRegistro = nuevaFechaRegistro;
				_context.Update(eleccion);
				await _context.SaveChangesAsync();
				TempData["AlertMessage"] = $"Fecha de registro cambiada exitosamente a: {eleccion.FechaRegistro}.";
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(ex.Message, "Ocurrió un error al cambiar la fecha de registro");
			}

			return RedirectToAction(nameof(ListadoEleccion));
		}


		//APARTADO DE ELIMINAR 
		public async Task<IActionResult> Eliminar(int? id)
		{
			if (id == null || _context.Elecciones == null)
			{
				return View();
			}

			var eleccion = await _context.Elecciones
				.FirstOrDefaultAsync(m => m.IdEleccion == id);

			if (eleccion == null)
			{
				return NotFound();
			}

			try
			{
				_context.Elecciones.Remove(eleccion);
				await _context.SaveChangesAsync();
				TempData["AlertMessage"] = "Eleccion eliminado exitosamente!!";
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(ex.Message, "Ocurrio un error, no se pudo eliminar el registro");

			}

			return RedirectToAction(nameof(ListadoEleccion));

		}



	}
}
