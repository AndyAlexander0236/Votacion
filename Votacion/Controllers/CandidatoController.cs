using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Votacion.Models;
using Votacion.Models.Entidades;

namespace Votacion.Controllers
{
    public class CandidatoController : Controller
    {
        private readonly LibreriaContext _context;

        public CandidatoController(LibreriaContext context)
        {
            _context = context;
        }

		public async Task<IActionResult> ListadoCandidato()
		{
			return View(await _context.Candidatos.ToListAsync());
		}

		public IActionResult Crear()
		{

			return View();
		}

		[HttpPost]

		public async Task<IActionResult> Crear(Candidato candidato)
		{

			if (ModelState.IsValid)
			{

				_context.Add(candidato);
				await _context.SaveChangesAsync();
				TempData["Alert Message"] = "Candidato Creado exitosamente";
				return RedirectToAction("ListadoCandidato");

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
			if (id == null || _context.Candidatos == null)
			{
				return NotFound();
			}

			var candidato = await _context.Candidatos.FindAsync(id);
			if (candidato == null)
			{
				return NotFound();
			}
			return View(candidato);
		}

		[HttpPost]
		public async Task<IActionResult> Editar(int id, Candidato candidato)
		{
			if (id != candidato.IdCandidato)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(candidato);
					await _context.SaveChangesAsync();
					TempData["AlertMessage"] = "Candidato actualizado " +
						"exitosamente!!!";
					return RedirectToAction("ListadoCandidato");
				}
				catch (Exception ex)
				{

					ModelState.AddModelError(ex.Message, "Ocurrio un error " +
						"al actualizar");
				}
			}
			return View(candidato);
		}

		public async Task<IActionResult> Eliminar(int? id)
		{
			if (id == null || _context.Candidatos == null)
			{
				return View();
			}

			var candidato = await _context.Candidatos
				.FirstOrDefaultAsync(m => m.IdCandidato == id);

			if (candidato == null)
			{
				return NotFound();
			}

			try
			{
				_context.Candidatos.Remove(candidato);
				await _context.SaveChangesAsync();
				TempData["AlertMessage"] = "Candidato eliminado exitosamente!!";
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(ex.Message, "Ocurrio un error, no se pudo eliminar el registro");

			}

			return RedirectToAction(nameof(ListadoCandidato));

		}

		[HttpPost]
		public async Task<IActionResult> CambiarEstado(int id)
		{
			var candidato = await _context.Candidatos.FindAsync(id);

			if (candidato == null)
			{
				return NotFound();
			}

			try
			{
				candidato.Activo = !candidato.Activo; // Cambiar el estado activo/desactivo
				_context.Update(candidato);
				await _context.SaveChangesAsync();
				TempData["AlertMessage"] = $"Estado del Candidato {candidato.NombreCandidato} " +
					$"cambiado exitosamente a {(candidato.Activo ? "Activo" : "Inactivo")}.";
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(ex.Message, "Ocurrió un error al cambiar el estado del candidato");
			}

			return RedirectToAction(nameof(ListadoCandidato));
		}


		[HttpPost]
		public async Task<IActionResult> CambiarRutaImagen(int id, string nuevaRutaImagen)
		{
			var candidato = await _context.Candidatos.FindAsync(id);

			if (candidato == null)
			{
				return NotFound();
			}

			try
			{
				candidato.RutaImagen = nuevaRutaImagen;
				_context.Update(candidato);
				await _context.SaveChangesAsync();
				TempData["AlertMessage"] = $"Ruta de imagen del Candidato {candidato.NombreCandidato} " +
					$"cambiada exitosamente a {candidato.RutaImagen}.";
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(ex.Message, "Ocurrió un error al cambiar la ruta de imagen del candidato");
			}

			return RedirectToAction(nameof(ListadoCandidato));
		}

		[HttpPost]
		public async Task<IActionResult> CambiarMensaje(int id, string nuevoMensaje)
		{
			var candidato = await _context.Candidatos.FindAsync(id);

			if (candidato == null)
			{
				return NotFound();
			}

			try
			{
				candidato.Mensaje = nuevoMensaje;
				_context.Update(candidato);
				await _context.SaveChangesAsync();
				TempData["AlertMessage"] = $"Mensaje del Candidato {candidato.NombreCandidato} " +
					$"cambiado exitosamente a: {candidato.Mensaje}.";
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(ex.Message, "Ocurrió un error al cambiar el mensaje del candidato");
			}

			return RedirectToAction(nameof(ListadoCandidato));
		}


		[HttpPost]
		public async Task<IActionResult> CambiarFechaRegistro(int id, DateTime nuevaFechaRegistro)
		{
			var candidato = await _context.Candidatos.FindAsync(id);

			if (candidato == null)
			{
				return NotFound();
			}

			try
			{
				candidato.FechaRegistro = nuevaFechaRegistro;
				_context.Update(candidato);
				await _context.SaveChangesAsync();
				TempData["AlertMessage"] = $"Fecha de registro del Candidato {candidato.NombreCandidato} " +
					$"cambiada exitosamente a: {candidato.FechaRegistro}.";
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(ex.Message, "Ocurrió un error al cambiar la fecha de registro del candidato");
			}

			return RedirectToAction(nameof(ListadoCandidato));
		}


	}
}
