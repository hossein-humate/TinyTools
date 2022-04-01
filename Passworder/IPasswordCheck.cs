namespace Basemap.Identity.Common
{
    /// <summary>
    /// Contain methods to determine password complexity and strong
    /// </summary>
    public interface IPasswordCheck
    {
        /// <summary>
        /// Check given input value has minimum 8 characters,one or more digit, one or more lower case character,
        /// one or more upper case character and one or more special character.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="input">Represent the input string</param>
        /// <returns>True when all complexity passed</returns>
        public bool HasFullComplexity(string input);

        /// <summary>
        /// Check the given new password value not same as current password
        /// </summary>
        /// <param name="newPassword">Represent the new Password value</param>
        /// <param name="salt">Represent the Current password salt value</param>
        /// <param name="hash">Represent the Current password hash value</param>
        /// <returns>True if the new password value is not same as current stored password value.</returns>
        public bool NotEqual(string newPassword, string salt, string hash);

        /// <summary>
        /// Check the given string value has minimum Length
        /// </summary>
        /// <param name="input">Represent the input string</param>
        /// <param name="min">Represent the minimum characters Length</param>
        /// <returns>True if the minimum Length is equal or more than the given 'min' or default 8 characters</returns>
        public bool HasMinLength(string input, int min = 8);

        /// <summary>
        /// Check the given string value has maximum Length
        /// </summary>
        /// <param name="input">Represent the input string</param>
        /// <param name="max">Represent the maximum characters Length</param>
        /// <returns>True if the maximum Length is equal or less than the given 'max' or default 16 characters</returns>
        public bool HasMaxLength(string input, int max = 16);

        /// <summary>
        /// Check the given string value has any digit 
        /// </summary>
        /// <param name="input">Represent the input string</param>
        /// <returns>True if the given input string value contains at lease one digit</returns>
        public bool HasDigit(string input);

        /// <summary>
        /// Check the given string value has any lower case character 
        /// </summary>
        /// <param name="input">Represent the input string</param>
        /// <returns>True if the given input string value contains at lease one lower case character</returns>
        public bool HasLowerCase(string input);

        /// <summary>
        /// Check the given string value has any upper case character 
        /// </summary>
        /// <param name="input">Represent the input string</param>
        /// <returns>True if the given input string value contains at lease one upper case character</returns>
        public bool HasUpperCase(string input);

        /// <summary>
        /// Check the given string value has any special character 
        /// </summary>
        /// <param name="input">Represent the input string</param>
        /// <returns>True if the given input string value contains at lease one special character</returns>
        public bool HasSpecialChar(string input);

        /// <summary>
        /// Check the given string value has space char
        /// </summary>
        /// <param name="input">Represent the input string</param>
        /// <returns>True if the given input string contain space character</returns>
        public bool HasSpaceChar(string input);
    }
}
