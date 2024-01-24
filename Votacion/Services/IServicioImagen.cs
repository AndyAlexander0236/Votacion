using Votacion.Models.Entidades;

namespace Votacion.Services
{
	public interface IServicioImagen
	{
		Task<string> SubirImagen(string archivo, string nombre);
		Task<string> SubirImagen(Stream image, string fileName);
		Task<string> SubirImagen(Stream imageStream, object name);

	}
}
