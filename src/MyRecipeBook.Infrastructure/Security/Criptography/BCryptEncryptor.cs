using MyRecipeBook.Domain.Criptography;

namespace MyRecipeBook.Infrastructure.Security.Criptography
{
    public class BCryptEncryptor : IPasswordEncryptor
    {
        public string Encrypt(string password)
        {
            var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
            return passwordHash;
        }

        public bool ValidPassword(string passwordToValidate, string password)
        {
            var isValid = false;
            try
            {
                isValid = BCrypt.Net.BCrypt.EnhancedVerify(passwordToValidate, password);
            }

            catch
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
