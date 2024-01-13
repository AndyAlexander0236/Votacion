using Votacion.Models.Entidades;

namespace Votacion.Services
{
    public interface IServicioUsuario
    {
        Task<Usuario> GetUsuario(int IdUsuario, string NombreUsuario);
        Task<Usuario> SaveUsuario(Usuario usuario);
        Task<Usuario> GetUsuario(string NombreUsuario);
    }
}
