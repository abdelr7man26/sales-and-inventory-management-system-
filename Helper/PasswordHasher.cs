using System;
using System.Security.Cryptography;
using System.Text;

namespace Auto_Parts_Store.Helpers
{
    /// <summary>
    /// PBKDF2-SHA256 password hashing with a fixed application salt.
    ///
    /// Why not BCrypt/Argon2? Those require third-party NuGet packages.
    /// Rfc2898DeriveBytes ships with .NET 4.7.2 and is strong enough for
    /// a single-machine desktop POS when combined with 10,000 iterations.
    ///
    /// Migration path for existing plain-text passwords:
    ///   On first successful login (plain-text match fallback), re-hash and
    ///   update the stored value. After all users have logged in once,
    ///   remove the fallback.
    /// </summary>
    public static class PasswordHasher
    {
        // 10,000 iterations — NIST SP 800-132 minimum for SHA-256
        private const int    Iterations = 10_000;
        private const int    KeyLength  = 32; // 256-bit

        // Per-application salt mixed with the password before PBKDF2.
        // Change this value when deploying to a new client installation
        // (requires resetting all passwords if changed mid-deployment).
        private const string AppSalt = "AutoPartsStore_v1_Salt_2024";

        public static string Hash(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));

            byte[] saltBytes = Encoding.UTF8.GetBytes(AppSalt);
            using (var deriveBytes = new Rfc2898DeriveBytes(plainText, saltBytes, Iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = deriveBytes.GetBytes(KeyLength);
                return Convert.ToBase64String(hash);
            }
        }

        public static bool Verify(string plainText, string storedHash)
            => Hash(plainText) == storedHash;
    }
}
