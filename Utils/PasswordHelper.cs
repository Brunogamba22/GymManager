using System;
using System.Security.Cryptography;
using System.Text;

namespace GymManager.Utils
{
    public static class PasswordHelper
    {
        // Hashea la contraseña con SHA256
        public static string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        // Verifica si la contraseña ingresada coincide con el hash guardado
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            string enteredHash = HashPassword(enteredPassword);
            return enteredHash == storedHash;
        }
    }
}
