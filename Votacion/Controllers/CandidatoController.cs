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
			return View(await _context.Candidatos.ToListAsync());
		}

		public IActionResult Crear()
		{

			var elecciones = _context.Elecciones
		   .Select(e => new SelectListItem { Value = e.IdEleccion.ToString(), Text = e.Descripcion })
		   .ToList();

			// Inicializa la propiedad Elecciones en el modelo Candidato
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
					var usuarioExistente = await _context.Usuarios.FindAsync(id);

					if (usuarioExistente == null)
					{
						return NotFound();
					}

					// Actualizar propiedades del usuario existente
					usuarioExistente.NombreUsuario = usuario.NombreUsuario;
					usuarioExistente.CorreoUsuario = usuario.CorreoUsuario;
					usuarioExistente.ClaveUsuario = EncriptarClave(usuario.ClaveUsuario); // Encriptar la nueva contraseña
					usuarioExistente.Rol = usuario.Rol;

					_context.Update(usuarioExistente);
					await _context.SaveChangesAsync();

					TempData["AlertMessage"] = "Usuario actualizado exitosamente!!!";
					return RedirectToAction("ListadoUsuario");
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, $"Ocurrió un error al actualizar: {ex.Message}");
				}
			}

			return View(usuario);
		}

		private string EncriptarClave(string claveUsuario)
		{
			throw new NotImplementedException();
		}


		//EDITAR MENSAJE 
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


		

		//EDITAR FECHA DE REGISTRO
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
