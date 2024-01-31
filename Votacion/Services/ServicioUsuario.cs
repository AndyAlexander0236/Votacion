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
        public async Task<Usuario> GetUsuario(string CorreoUsuario, string ClaveUsuario, int Rol)
        {
            Usuario usuario = await _context.Usuarios.Where(u => u.CorreoUsuario == CorreoUsuario && u.ClaveUsuario == ClaveUsuario && u.IdRol == Rol).FirstOrDefaultAsync();

            return usuario;
        }
        public async Task<Usuario> GetUsuario(string NombreUsuario)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == NombreUsuario);
        }
        public async Task<Usuario> GetUsuarioPorCorreo(string Correo)
        {
            Usuario usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoUsuario == Correo);

            return usuario;
        }
       
        public async Task<Usuario> SaveUsuario(Usuario Usuario)
        {
            _context.Usuarios.Add(Usuario);
            await _context.SaveChangesAsync();
            return Usuario;
        }

   
    }
}
