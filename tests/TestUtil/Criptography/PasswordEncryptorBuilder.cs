using MyRecipeBook.Infrastructure.Security.Criptography;

namespace TestUtil.Criptography
{
    public static class PasswordEncryptorBuilder
    {
        public static BCryptEncryptor Build()
        {
            return new BCryptEncryptor();
        }
    }
}
