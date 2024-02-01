using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Votacion.Models;
using Votacion.Models.Entidades;
using Votacion.Services;

namespace Votacion.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly LibreriaContext _context;
        private readonly IServicioUsuario _ServicioUsuario;
        private readonly IServicioImagen _ServicioImagen;
        private readonly IServicioLista _ServicioLista;


        private string EncriptarClave(string clave)
        {

            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convertir la cadena de la contraseña en un arreglo de bytes
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(clave));

                // Convertir los bytes a una cadena hexadecimal
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }


        public UsuarioController(IServicioImagen ServicioImagen, IServicioUsuario ServicioUsuario, IServicioLista ServicioLista, LibreriaContext context)
        {
            _context = context;
            _ServicioImagen = ServicioImagen;
            _ServicioUsuario = ServicioUsuario;
            _ServicioLista = ServicioLista;

        }


        public async Task<IActionResult> ListadoUsuario()
        {
            return View(await _context.Usuarios.ToListAsync());
        }


        public async Task<IActionResult> Lista()
        {
            return View();

        }


        public IActionResult Crear()
        {

            var roles = _context.Roles.Select(e => new SelectListItem
            {
                Value = e.IdRol.ToString(),
                Text = e.Rol
            }).ToList();

            ViewData["Roles"] = roles;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Usuario usuario, IFormFile Imagen, int IdRol)
        {


            // Validar si no se ha seleccionado una imagen
            if (Imagen == null || Imagen.Length == 0)
            {
                ViewData["Mensaje"] = "Es necesario subir una foto.";
                return View();
            }

            // Validar si el usuario ya existe
            Usuario usuarioExistente = await _ServicioUsuario.GetUsuarioPorCorreo(usuario.CorreoUsuario);

            if (usuarioExistente != null)
            {
                ViewData["Mensaje"] = "El usuario ya existe. Por favor, elija otro correo electrónico.";
                return View();
            }

            Stream image = Imagen.OpenReadStream();
            string urlImagen = await _ServicioImagen.SubirImagen(image, Imagen.FileName);

            usuario.ClaveUsuario = Utilitarios.EncriptarClave(usuario.ClaveUsuario);
            usuario.URLFotoPerfil = urlImagen;

            // Asignar el rol seleccionado al usuario
            usuario.IdRol = IdRol;

            Usuario usuarioCreado = await _ServicioUsuario.SaveUsuario(usuario);

            if (usuarioCreado.IdUsuario > 0)
            {
                return RedirectToAction("ListadoUsuario", "Usuario");
            }

            ViewData["Mensaje"] = "No se pudo crear el usuario";

            // Si llegas aquí, algo salió mal, vuelve a cargar el formulario con el modelo y las elecciones.
            usuario.Rol = _context.Roles.Select(e => new SelectListItem
            {
                Value = e.IdRol.ToString(),
                Text = e.Rol
            });

            return View(usuario);
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

            var roles = _context.Roles.Select(e => new SelectListItem
            {
                Value = e.IdRol.ToString(),
                Text = e.Rol
            }).ToList();

            ViewData["Roles"] = roles;

            return View(usuario);


        }


        [HttpPost]
         public async Task<IActionResult> Editar(int id, Usuario usuario, IFormFile Imagen)
        {
            if (Imagen != null)
            {
                try
                {
                var usuarioExistente = await _context.Usuarios.FindAsync(id);

                    if (usuarioExistente == null)
                    {
                        return NotFound();
                    }
                    // Subir la nueva imagen con el nuevo nombre único
                    Stream image = Imagen.OpenReadStream();
                    string urlImagen = await _ServicioImagen.SubirImagen(image,Imagen.FileName);
                    usuarioExistente.URLFotoPerfil = urlImagen;

                    // Actualizar los demás campos del libro
                    usuarioExistente.DocumentoIdentidad = usuario.DocumentoIdentidad;
                    usuarioExistente.NombreUsuario = usuario.NombreUsuario;
                    usuarioExistente.ApellidoUsuario = usuario.ApellidoUsuario;
                    usuarioExistente.CorreoUsuario = usuario.CorreoUsuario;
                    usuarioExistente.ClaveUsuario = usuario.ClaveUsuario;
                    usuarioExistente.IdRol  = usuario.IdRol;

                    _context.Update(usuarioExistente);
                    await _context.SaveChangesAsync();

                    TempData["AlertMessage"] = "Libro actualizado exitosamente!!!";
                    return RedirectToAction("ListadoUsuario");
                }
                catch (Exception ex)
                {
                    TempData["AlertMessage"] = ex.Message;
                    return RedirectToAction("ListadoUsuario");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido un error");
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





    }
}
