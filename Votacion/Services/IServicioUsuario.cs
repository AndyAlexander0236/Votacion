using Votacion.Models.Entidades;

namespace Votacion.Services
{
    public interface IServicioUsuario
    {
        Task<Usuario> GetUsuario(String CorreoUsuario, String ClaveUsuario, int Rol );
        Task<Usuario> SaveUsuario(Usuario Usuario);
        Task<Usuario> GetUsuario(String NombreUsuario);
        Task<Usuario> GetUsuarioPorCorreo(string Correo);

    }
}
