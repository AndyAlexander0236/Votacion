using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Votacion.Models;
using Votacion.Models.Entidades;

namespace Votacion.Controllers
{
	public class UsuarioController : Controller
	{
		private readonly LibreriaContext _context;

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


        public UsuarioController(LibreriaContext context)
		{
			_context = context;
		}


		public async Task<IActionResult> ListadoUsuario()
		{
			return View(await _context.Usuarios.ToListAsync());
		}

		public IActionResult Crear()
		{

			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Crear(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
             
                // Encriptar la contraseña antes de almacenarla
                usuario.ClaveUsuario = EncriptarClave(usuario.ClaveUsuario);
				
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = "Usuario creado exitosamente";
                return RedirectToAction("ListadoUsuario");
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Ha ocurrido un error");
            }

            return View();
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
			return View(usuario);
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
                    _context.Update(usuario);
					await _context.SaveChangesAsync();
					TempData["AlertMessage"] = "Usuario actualizado " +
						"exitosamente!!!";
					return RedirectToAction("ListadoUsuario");
				}
				catch (Exception ex)
				{

					ModelState.AddModelError(ex.Message, "Ocurrio un error " +
						"al actualizar");
				}
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
