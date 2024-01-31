using Votacion.Models.Entidades;

namespace Votacion.Services
{
	public interface IServicioCandidato
    {
        Task<Candidato> GetCandidato(int IdCandidato, string NombreCandidato);
        Task<Candidato> SaveCandidato(Candidato candidato);
        Task<Candidato> GetCandidato(string NombreCandidato);


    }
}
