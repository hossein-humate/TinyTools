using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Basemap.Identity.Common
{
    /// <inheritdoc/>
    public class PasswordCheck : IPasswordCheck
    {
        /// <inheritdoc/>
        public bool HasFullComplexity(string input)
        {
            return HasMinLength(input)
                && HasDigit(input)
                && HasLowerCase(input)
                && HasUpperCase(input)
                && HasSpecialChar(input)
                && !HasSpaceChar(input);
        }

        /// <inheritdoc/>
        public bool NotEqual(string newPassword, string salt, string hash)
        {
            return NotNull(newPassword)
                  && !ValidateHash(newPassword,salt, hash);
        }

        /// <inheritdoc/>
        public bool HasMinLength(string input, int min = 8)
        {
            return NotNull(input) && input.Length >= min;
        }

        /// <inheritdoc/>
        public bool HasMaxLength(string input, int max = 16)
        {
            return NotNull(input) && input.Length <= max;
        }

        /// <inheritdoc/>
        public bool HasDigit(string input)
        {
            return NotNull(input) && Regex.Match(input, @"\d+").Success;
        }

        /// <inheritdoc/>
        public bool HasLowerCase(string input)
        {
            return NotNull(input) && Regex.Match(input, @"[a-z]").Success;
        }

        /// <inheritdoc/>
        public bool HasUpperCase(string input)
        {
            return NotNull(input) && Regex.Match(input, @"[A-Z]").Success;
        }

        /// <inheritdoc/>
        public bool HasSpecialChar(string input)
        {
            return NotNull(input) && Regex.Match(input, @".[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]").Success;
        }

        /// <inheritdoc/>
        public bool HasSpaceChar(string input)
        {
            return NotNull(input) && Regex.Match(input, @"\s+").Success;
        }

        public static string CreateHash(string value, string salt)
        {
            var valueBytes = KeyDerivation.Pbkdf2(value, Encoding.UTF8.GetBytes(salt),
                KeyDerivationPrf.HMACSHA512, 10000, 256 / 8);
            return Convert.ToBase64String(valueBytes);
        }

        public static bool ValidateHash(string value, string salt, string hash)
        {
            return !string.IsNullOrEmpty(salt) && !string.IsNullOrEmpty(hash) && CreateHash(value, salt) == hash;
        }

        public static string GenerateSecret(int size = 16)
        {
            var randomBytes = new byte[size];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        private static bool NotNull(string input)
        {
            return !string.IsNullOrEmpty(input);
        }
    }
}
