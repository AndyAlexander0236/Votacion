﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Votacion.Models;
using Votacion.Models.Entidades;

namespace Votacion.Controllers
{
	public class VotanteController : Controller
	{
		private readonly LibreriaContext _context;

		public VotanteController(LibreriaContext context)
		{
			_context = context;
		}


		public async Task<IActionResult> ListadoVotante()
		{
			return View(await _context.Votantes.ToListAsync());
		}

		public IActionResult Crear()
		{
			return View();
		}



		[HttpPost]
		public async Task<IActionResult> Crear(Votante votante)
		{

			try
			{
				_context.Add(votante);
				await _context.SaveChangesAsync();
				TempData["AlertMessage"] = "Votante creado exitosamente";
				return RedirectToAction("ListadoVotante");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, $"Ha ocurrido un error: {ex.Message}");
			}

			return View(votante);
		}

		[HttpGet]
		public async Task<IActionResult> Editar(int? id)
		{
			if (id == null || _context.Votantes == null)
			{
				return NotFound();
			}

			var votante = await _context.Votantes.FindAsync(id);
			if (votante == null)
			{
				return NotFound();
			}
			return View(votante);
		}

		[HttpPost]
		public async Task<IActionResult> Editar(int id, Votante votante)
		{
			if (id != votante.IdVotante)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(votante);
					await _context.SaveChangesAsync();
					TempData["AlertMessage"] = "Votante actualizado " +
						"exitosamente!!!";
					return RedirectToAction("ListadoVotante");
				}
				catch (Exception ex)
				{

					ModelState.AddModelError(ex.Message, "Ocurrio un error " +
						"al actualizar");
				}
			}
			return View(votante);
		}


		//EDITAR FECHA DE REGISTRO
		[HttpPost]
		public async Task<IActionResult> CambiarFechaRegistro(int id, DateTime nuevaFechaRegistro)
		{
			var votante = await _context.Votantes.FindAsync(id);

			if (votante == null)
			{
				return NotFound();
			}

			try
			{
				votante.FechaRegistro = nuevaFechaRegistro;
				_context.Update(votante);
				await _context.SaveChangesAsync();
				TempData["AlertMessage"] = $"Fecha de registro del Votante {votante.NombreVotante} " +
					$"cambiada exitosamente a: {votante.FechaRegistro}.";
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(ex.Message, "Ocurrió un error al cambiar la fecha de registro del candidato");
			}

			return RedirectToAction(nameof(ListadoVotante));
		}


		//APARTADO DE ELIMINAR
		public async Task<IActionResult> Eliminar(int? id)
		{
			if (id == null || _context.Votantes == null)
			{
				return View();
			}

			var votante = await _context.Votantes
				.FirstOrDefaultAsync(m => m.IdVotante == id);

			if (votante == null)
			{
				return NotFound();
			}

			try
			{
				_context.Votantes.Remove(votante);
				await _context.SaveChangesAsync();
				TempData["AlertMessage"] = "Votante eliminado exitosamente!!";
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(ex.Message, "Ocurrio un error, no se pudo eliminar el registro");

			}

			return RedirectToAction(nameof(ListadoVotante));

		}


	}
}
