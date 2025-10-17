using System;
using System.Security.Cryptography;
using System.Text;

namespace GymManager.Utils
{
    public static class PasswordHelper
    {
        // Genera un hash SHA256 para la contraseña
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (var sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes).Trim();
            }
        }

        // Verifica si la contraseña ingresada coincide con el hash guardado
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(enteredPassword))
                return false;

            string enteredHash = HashPassword(enteredPassword).Trim();
            string dbHash = storedHash.Trim();

            return string.Equals(enteredHash, dbHash, StringComparison.Ordinal);
        }
    }
}
