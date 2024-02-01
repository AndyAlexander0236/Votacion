using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
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
			var votaciones = await _context.Votaciones.ToListAsync();
			return View(votaciones);
		}

		public IActionResult Crear()
		{
			CargarListasDesplegables();
			return View();
		}

		

		// Método en el controlador para manejar la solicitud POST después de enviar el formulario de creación
		[HttpPost]
		public async Task<IActionResult> Crear(Votaciones votacion)
		{
			if (ModelState.IsValid)
			{
				try
				{
					// Lógica para guardar la votación en la base de datos
					_context.Add(votacion);
					await _context.SaveChangesAsync();
					TempData["AlertMessage"] = "Votacion creada exitosamente";
					return RedirectToAction("ListadoVotaciones");
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(String.Empty, $"Ha ocurrido un error: {ex.Message}");
				}
			}

			// Recargar las listas desplegables en caso de que haya errores de validación
			CargarListasDesplegables();
			return View(votacion);
		}

		// Método privado para cargar las listas desplegables necesarias para el formulario
		private void CargarListasDesplegables()
		{
			ViewData["Candidatos"] = new SelectList(_context.Candidatos
				.Select(c => new { IdCandidato = c.IdCandidato, NombreCompleto = $"{c.NombreCandidato} {c.ApellidoCandidato}" }), "IdCandidato", "NombreCompleto");

			ViewData["Elecciones"] = new SelectList(_context.Elecciones
				.Select(e => new { IdEleccion = e.IdEleccion, Descripcion = e.Descripcion }), "IdEleccion", "Descripcion");
		}



		[HttpGet]
		public async Task<IActionResult> Editar(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var votaciones = await _context.Votaciones.FindAsync(id);

			if (votaciones == null)
			{
				return NotFound();
			}

			CargarListasDesplegables();
			return View(votaciones);
		}

		[HttpPost]
		public async Task<IActionResult> Editar(int id, Votaciones votaciones)
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
					TempData["AlertMessage"] = "Votacion actualizada exitosamente";
					return RedirectToAction("ListadoVotaciones");
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(String.Empty, $"Ha ocurrido un error: {ex.Message}");
				}
			}

			CargarListasDesplegables();
			return View(votaciones);
		}


		[HttpPost]
		public IActionResult ValidarDocumento(string documentoIdentidad)
		{
			var votante = _context.Votantes.FirstOrDefault(v => v.DocumentoIdentidad == documentoIdentidad);

			if (votante != null)
			{
				return Json(new { success = true, nombreVotante = votante.NombreVotante });
			}
			else
			{
				return Json(new { success = false });
			}
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



		

