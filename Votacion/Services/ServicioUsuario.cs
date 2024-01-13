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


        public async Task<Usuario> GetUsuario(int IdUsuario, string NombreUsuario)
        {
           Usuario usuario = await _context.Usuarios.Where(u => u.IdUsuario == IdUsuario
              && u.NombreUsuario == NombreUsuario).FirstOrDefaultAsync();

            return usuario;
        }

        public async Task<Usuario> GetUsuario(string NombreUsuario)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == NombreUsuario);

        }

        public async Task<Usuario> SaveUsuario(Usuario usuario)
        {
            _context.Usuarios .Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }
    }
}
