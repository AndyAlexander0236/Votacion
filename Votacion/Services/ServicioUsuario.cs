using Microsoft.EntityFrameworkCore;
using Votacion.Models;
using Votacion.Models.Entidades;

namespace Votacion.Services
{
    public class ServicioUsuario : IServicioUsuario
    {
        private readonly LibreriaContext _context;
        public ServicioUsuario(LibreriaContext context)
        {
            _context = context;
        }
        public async Task<Usuario> GetUsuario(string correo, string clave)
        {
            Usuario usuario = await _context.Usuarios.Where(u => u.CorreoUsuario == correo && u.ClaveUsuario == clave).FirstOrDefaultAsync();

            return usuario;
        }
        public async Task<Usuario> GetUsuario(string nombre_usuario)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == nombre_usuario);
        }
        public async Task<Usuario> GetUsuarioPorCorreo(string correo)
        {
            Usuario usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.CorreoUsuario == correo);

            return usuario;
        }
        public async Task<Usuario> SaveUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }
    }
}
