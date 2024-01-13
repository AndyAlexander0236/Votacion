using Microsoft.EntityFrameworkCore;
using Votacion.Models;
using Votacion.Models.Entidades;

namespace Votacion.Services
{
    public class ServicioCandidato : IServicioCandidato
    {
        private readonly LibreriaContext _context;

        public ServicioCandidato(LibreriaContext context)
        {
            _context = context;
        }

        public async Task<Candidato> GetCandidato(int IdCandidato, string NombreCandidato)
        {
            Candidato candidato = await _context.Candidatos.Where(u => u.IdCandidato == IdCandidato
            && u.NombreCandidato == NombreCandidato).FirstOrDefaultAsync();

            return candidato;
        }

        public async Task<Candidato> GetCandidato(string NombreCandidato)
        {
            return await _context.Candidatos.FirstOrDefaultAsync(u => u.NombreCandidato == NombreCandidato);


        }

        public async  Task<Candidato> SaveCandidato(Candidato candidato)
        {
            _context.Candidatos.Add(candidato);
            await _context.SaveChangesAsync();
            return candidato;

        }
    }
}
