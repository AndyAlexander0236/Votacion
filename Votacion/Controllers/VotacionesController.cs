using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Votacion.Models;
using Votacion.Models.Entidades;

namespace Votacion.Controllers
{
	public class VotacionesController : Controller
	{
		private readonly LibreriaContext _context;

		public VotacionesController(LibreriaContext context)
		{
			_context = context;
		}


		public async Task<IActionResult> ListadoVotaciones()
		{
			return View(await _context.Votaciones.ToListAsync());
		}

		public IActionResult Crear()
		{

            // Cargar la lista de candidatos con nombre y apellido combinados
            ViewData["Candidatos"] = new SelectList(_context.Candidatos.Select(c => new
            {
                IdCandidato = c.IdCandidato,
                NombreCompleto = $"{c.NombreCandidato} {c.ApellidoCandidato}"
            }), "IdCandidato", "NombreCompleto");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Votaciones votacion)
        {
            if (ModelState.IsValid)
            {
                // Aquí puedes usar votacion.IdCandidato para obtener el Id del candidato seleccionado

                // Resto del código para guardar la votación
                _context.Add(votacion);
                await _context.SaveChangesAsync();

                TempData["Alert Message"] = "Votacion Creado exitosamente";
                return RedirectToAction("ListadoVotaciones");
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
			if (id == null || _context.Votaciones == null)
			{
				return NotFound();
			}

			var votaciones = await _context.Votaciones.FindAsync(id);
			if (votaciones == null)
			{
				return NotFound();
			}
			return View(votaciones);
		}

		[HttpPost]
		public async Task<IActionResult> Editar(int id, Votaciones votaciones )
		{
			if (id != votaciones.IdVotacion)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(votaciones);
					await _context.SaveChangesAsync();
					TempData["AlertMessage"] = "Votacion actualizado " +
						"exitosamente!!!";
					return RedirectToAction("ListadoVotaciones");
				}
				catch (Exception ex)
				{

					ModelState.AddModelError(ex.Message, "Ocurrio un error " +
						"al actualizar");
				}
			}
			return View(votaciones);
		}


		//EDITAR FECHA DE REGISTRO
		[HttpPost]
		public async Task<IActionResult> CambiarFechaRegistro(int id, DateTime nuevaFechaRegistro)
		{
			var votaciones = await _context.Votaciones.FindAsync(id);

			if (votaciones == null)
			{
				return NotFound();
			}

			try
			{
				votaciones.FechaRegistro = nuevaFechaRegistro;
				_context.Update(votaciones);
				await _context.SaveChangesAsync();
				TempData["AlertMessage"] = $"Fecha de registro cambiada exitosamente a: {votaciones.FechaRegistro}.";
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(ex.Message, "Ocurrió un error al cambiar la fecha de registro");
			}

			return RedirectToAction(nameof(ListadoVotaciones));
		}



		//APARTADO DE ELIMINAR
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
				TempData["AlertMessage"] = "Votacion eliminado exitosamente!!";
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(ex.Message, "Ocurrio un error, no se pudo eliminar el registro");

			}

			return RedirectToAction(nameof(ListadoVotaciones));

		}


		

	}
}
