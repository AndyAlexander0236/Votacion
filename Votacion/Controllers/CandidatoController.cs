using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Votacion.Models;
using Votacion.Models.Entidades;
using Votacion.Services;

namespace Votacion.Controllers
{
	public class CandidatoController : Controller
    {
        private readonly IServicioImagen _ServicioImagen;

        private readonly LibreriaContext _context;

        public CandidatoController(IServicioImagen ServicioImagen, LibreriaContext context)
        {
			_ServicioImagen = ServicioImagen;
		   _context = context;
        }

		public async Task<IActionResult> ListadoCandidato()
		{
            // Incluye la propiedad de navegación Eleccion al cargar los candidatos
            var candidatos = await _context.Candidatos.Include(c => c.Eleccion).ToListAsync();

            return View(candidatos);
        }

		public IActionResult Crear()
		{

            var elecciones = _context.Elecciones
       .Select(e => new SelectListItem { Value = e.IdEleccion.ToString(), Text = e.Descripcion })
       .ToList();

            var candidato = new Candidato
            {
                Elecciones = elecciones
            };

            return View(candidato);
        }
        [HttpPost]
        public async Task<IActionResult> Crear(Candidato candidato, IFormFile Imagen)
        {
            // Validar si no se ha seleccionado una imagen
            if (Imagen == null || Imagen.Length == 0)
            {
                ViewData["Mensaje"] = "Es necesario subir una foto.";
                return View();
            }

            try
            {
                // Subir la imagen y obtener la URL
                Stream image = Imagen.OpenReadStream();
                string urlImagen = await _ServicioImagen.SubirImagen(image, Imagen.FileName);
                candidato.ImgCandidato = urlImagen;

                // Asignar la elección seleccionada al Candidato
                candidato.Eleccion = await _context.Elecciones.FindAsync(candidato.IdEleccion);


                // Agregar el candidato a la base de datos
                _context.Add(candidato);
                await _context.SaveChangesAsync();

                TempData["AlertMessage"] = "Candidato creado exitosamente";
                return RedirectToAction("ListadoCandidato");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Ha ocurrido un error: {ex.Message}");
            }

            // Recargar la lista de elecciones en caso de error
            candidato.Elecciones = _context.Elecciones
                .Select(e => new SelectListItem { Value = e.IdEleccion.ToString(), Text = e.Descripcion })
                .ToList();

            return View(candidato);
        }


		[HttpGet]
		public async Task<IActionResult> Editar(int? id)
		{
			if (id == null)
			{
				return NotFound(); // Devuelve 404 Not Found si el id no está especificado
			}

			var candidato = await _context.Candidatos
				.Include(c => c.Eleccion) // Incluye la propiedad de navegación Eleccion
				.FirstOrDefaultAsync(c => c.IdCandidato == id);

			if (candidato == null)
			{
				return NotFound(); // Devuelve 404 Not Found si no se encuentra el candidato
			}

			// Cargar la lista de elecciones en ViewData para que esté disponible en la vista
			ViewData["Elecciones"] = new SelectList(_context.Elecciones, "IdEleccion", "Descripcion", candidato.IdEleccion);

			return View(candidato);
		}


		[HttpPost]
		public async Task<IActionResult> Editar(int id, Candidato candidato, IFormFile Imagen)
		{
			if (id != candidato.IdCandidato)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					// Validar si no se ha seleccionado una nueva imagen
					if (Imagen != null && Imagen.Length > 0)
					{
						// Subir la nueva imagen y obtener la URL
						Stream image = Imagen.OpenReadStream();
						string urlImagen = await _ServicioImagen.SubirImagen(image, Imagen.FileName);
						candidato.ImgCandidato = urlImagen;
					}

					// Obtener la elección seleccionada
					candidato.Eleccion = await _context.Elecciones.FindAsync(candidato.IdEleccion);

					// Actualizar propiedades del candidato existente
					var candidatoExistente = await _context.Candidatos.FindAsync(id);
					if (candidatoExistente == null)
					{
						return NotFound();
					}

					candidatoExistente.NombreCandidato = candidato.NombreCandidato;
					candidatoExistente.ApellidoCandidato = candidato.ApellidoCandidato;
					candidatoExistente.Mensaje = candidato.Mensaje;
					candidatoExistente.FechaRegistro = candidato.FechaRegistro;
					candidatoExistente.Eleccion = candidato.Eleccion;

					_context.Update(candidatoExistente);
					await _context.SaveChangesAsync();

					TempData["AlertMessage"] = "Candidato actualizado exitosamente!!!";
					return RedirectToAction("ListadoCandidato");
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, $"Ocurrió un error al actualizar: {ex.Message}");
				}
			}

			// Recargar la lista de elecciones en caso de error
			candidato.Elecciones = _context.Elecciones
				.Select(e => new SelectListItem { Value = e.IdEleccion.ToString(), Text = e.Descripcion })
				.ToList();

			return View(candidato);
		}


		//APARTADO DE ELIMINAR
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

		

	}
}
