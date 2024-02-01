// EleccionController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Votacion.Models;
using Votacion.Models.Entidades;

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
		try
		{
			if (ModelState.IsValid)
			{
				_context.Add(eleccion);
				await _context.SaveChangesAsync();
				TempData["AlertMessage"] = "Eleccion creado exitosamente";
				return RedirectToAction(nameof(ListadoEleccion));
			}
		}
		catch (Exception ex)
		{
			ModelState.AddModelError(string.Empty, $"Ha ocurrido un error: {ex.Message}");
		}

		return View(eleccion);
	}

	[HttpGet]
	public async Task<IActionResult> Editar(int? id)
	{
		if (id == null)
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

		try
		{
			if (ModelState.IsValid)
			{
				_context.Update(eleccion);
				await _context.SaveChangesAsync();
				TempData["AlertMessage"] = "Eleccion actualizado exitosamente";
				return RedirectToAction(nameof(ListadoEleccion));
			}
		}
		catch (Exception ex)
		{
			ModelState.AddModelError(string.Empty, $"Ocurrió un error al actualizar: {ex.Message}");
		}

		return View(eleccion);
	}

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
			eleccion.Activo = !eleccion.Activo;
			_context.Update(eleccion);
			await _context.SaveChangesAsync();
			TempData["AlertMessage"] = $"Estado de la elección cambiado exitosamente a {(eleccion.Activo ? "Activo" : "Inactivo")}.";
		}
		catch (Exception ex)
		{
			ModelState.AddModelError(string.Empty, $"Ocurrió un error al cambiar el estado de la elección: {ex.Message}");
		}

		return RedirectToAction(nameof(ListadoEleccion));
	}

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
			if (ModelState.IsValid)
			{
				eleccion.FechaRegistro = nuevaFechaRegistro;
				_context.Update(eleccion);
				await _context.SaveChangesAsync();
				TempData["AlertMessage"] = $"Fecha de registro cambiada exitosamente a: {eleccion.FechaRegistro}.";
			}
			else
			{
				ModelState.AddModelError("FechaRegistro", "La fecha proporcionada no es válida.");
			}
		}
		catch (Exception ex)
		{
			ModelState.AddModelError(string.Empty, $"Ocurrió un error al cambiar la fecha de registro: {ex.Message}");
		}

		return RedirectToAction(nameof(ListadoEleccion));
	}

	public async Task<IActionResult> Eliminar(int? id)
	{
		if (id == null)
		{
			return NotFound();
		}

		var eleccion = await _context.Elecciones.FirstOrDefaultAsync(m => m.IdEleccion == id);

		if (eleccion == null)
		{
			return NotFound();
		}

		try
		{
			_context.Elecciones.Remove(eleccion);
			await _context.SaveChangesAsync();
			TempData["AlertMessage"] = "Eleccion eliminado exitosamente";
		}
		catch (Exception ex)
		{
			ModelState.AddModelError(string.Empty, $"Ocurrió un error, no se pudo eliminar el registro: {ex.Message}");
		}

		return RedirectToAction(nameof(ListadoEleccion));
	}
}
