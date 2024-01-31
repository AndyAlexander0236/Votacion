using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Votacion.Models;
using Votacion.Models.Entidades;


namespace Votacion.Services
{
    public class ServicioLista : IServicioLista
    {
        private readonly LibreriaContext _context;

        public ServicioLista(LibreriaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetListaUsuario()
        {
            List<SelectListItem> list = await _context.Usuarios.Select(x => new SelectListItem
            {
                Text = x.NombreUsuario,
                Value = $"{x.IdUsuario}"
            })
            .OrderBy(x => x.Text)
            .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un Usuario....]",
                Value = "0"

            });
            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetListaCandidato()
        {
            List<SelectListItem> list = await _context.Candidatos.Select(x => new SelectListItem
            {
                Text = x.NombreCandidato,
                Value = $"{x.IdCandidato}"
            })
            .OrderBy(x => x.Text)
            .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un Candidato....]",
                Value = "0"

            });
            return list;
        }


        public async Task<IEnumerable<SelectListItem>> GetListaRol()
        {
            List<SelectListItem> list = await _context.Roles.Select(x => new SelectListItem
            {
                Text = x.Rol,
                Value = $"{x.IdRol}"
            })
            .OrderBy(x => x.Text)
            .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un Rol....]",
                Value = "0"

            });
            return list;
        }



    }
}

