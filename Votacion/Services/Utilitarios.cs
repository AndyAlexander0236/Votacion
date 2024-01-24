using System.Security.Cryptography;
using System.Text;

namespace Votacion.Services
{
    public class Utilitarios
    {
        public static string EncriptarClave(string clave)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytesClave = Encoding.UTF8.GetBytes(clave);
                byte[] hashBytes = sha256.ComputeHash(bytesClave);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); //formato hexadecimal
                }
                return sb.ToString();
            }
        }

    }
}
