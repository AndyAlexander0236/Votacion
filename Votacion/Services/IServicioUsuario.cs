using Votacion.Models.Entidades;

namespace Votacion.Services
{
    public interface IServicioUsuario
    {
        Task<Usuario> GetUsuario(string CorreoUsuario, string ClaveUsuario);
        Task<Usuario> SaveUsuario(Usuario NombreUsuario);
        Task<Usuario> GetUsuario(string NombreUsuario);
        Task<Usuario> GetUsuarioPorCorreo(String CorreoUsuario);

    }
}
