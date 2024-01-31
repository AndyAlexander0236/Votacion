using Microsoft.AspNetCore.Mvc.Rendering;

namespace Votacion.Services
{
    public interface IServicioLista
    {
        Task<IEnumerable<SelectListItem>>
            GetListaUsuario();

        Task<IEnumerable<SelectListItem>>
            GetListaCandidato();

        Task<IEnumerable<SelectListItem>>
           GetListaRol();
    }
}
