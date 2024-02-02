using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
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
            // Obtener todas las votaciones con datos relacionados
            var votacionesConDatosRelacionados = await _context.Votaciones
                .Include(v => v.Candidato)
                .Include(v => v.Eleccion)
                .Include(v => v.Votante)
                .ToListAsync();

            return View(votacionesConDatosRelacionados);
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

            CargarListasDesplegables();
            return View();
        }



        // Método en el controlador para manejar la solicitud POST después de enviar el formulario de creación
        [HttpPost]
        public async Task<IActionResult> Crear(Votaciones votacion)
        {
            //if (ModelState.IsValid)
            //{
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
            //}

            // Recargar las listas desplegables en caso de que haya errores de validación
            CargarListasDesplegables();


            // Recargar la lista de elecciones en caso de error
            votacion.Elecciones = _context.Elecciones
                .Select(e => new SelectListItem { Value = e.IdEleccion.ToString(), Text = e.Descripcion })
                .ToList();

            return View(votacion);
        }



        // Método privado para cargar las listas desplegables necesarias para el formulario
        private void CargarListasDesplegables()
        {
            ViewData["Candidatos"] = new SelectList(_context.Candidatos
                .Select(c => new { IdCandidato = c.IdCandidato, NombreCompleto = $"{c.NombreCandidato} {c.ApellidoCandidato}" }), "IdCandidato", "NombreCompleto");

            ViewData["Elecciones"] = new SelectList(_context.Elecciones
                .Select(e => new { IdEleccion = e.IdEleccion, Descripcion = e.Descripcion }), "IdEleccion", "Descripcion");

            ViewData["Votantes"] = new SelectList(_context.Votantes
                .Select(c => new { IdVotante = c.IdVotante, EmitioVoto = $"{c.NombreVotante} {c.ApellidoVotante}" }), "IdVotante", "EmitioVoto");
        }



        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var votacion = await _context.Votaciones
                .Include(v => v.Candidato)
                .Include(v => v.Eleccion)
                .Include(v => v.Votante)
                .FirstOrDefaultAsync(m => m.IdVotacion == id);

            if (votacion == null)
            {
                return NotFound();
            }

            CargarListasDesplegables();
            return View(votacion);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, Votaciones votacion)
        {
            if (id != votacion.IdVotacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(votacion);
                    await _context.SaveChangesAsync();
                    TempData["AlertMessage"] = "Votación actualizada exitosamente";
                    return RedirectToAction(nameof(ListadoVotaciones));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Ha ocurrido un error: {ex.Message}");
                }
            }

            CargarListasDesplegables();
            return View(votacion);
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



        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var votacion = await _context.Votaciones.FindAsync(id);

            if (votacion == null)
            {
                return NotFound();
            }

            try
            {
                _context.Votaciones.Remove(votacion);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = "Votación eliminada exitosamente!!";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Ocurrió un error: {ex.Message}");
            }

            return RedirectToAction(nameof(ListadoVotaciones));
        }



    }
}