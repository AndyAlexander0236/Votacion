using Firebase.Auth;
using Firebase.Storage;

namespace Votacion.Services
{
    public class ServicioImagen : IServicioImagen
    {
        public async Task<string> SubirImagen(Stream archivo,
            string nombre)
        {
            string email = "logachologacho@gmail.com";
            string clave = "alexander0236";
            string ruta = "claseproyectoitq.appspot.com";
            string api_key = "AIzaSyDUVS3E0DkrLpiM7u7dY6uemdw-OgfCdxI";

            var auth = new FirebaseAuthProvider
                (new FirebaseConfig(api_key));
            var a = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
              ruta,
              new FirebaseStorageOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                  ThrowOnCancel = true
              })

               .Child("Fotos_Perfil")
               .Child(nombre)
               .PutAsync(archivo, cancellation.Token);

            var downloadURL = await task;
            return downloadURL;
        }

        public Task<string> SubirImagen(string archivo, string nombre)
        {
            throw new NotImplementedException();
        }
    }
}
