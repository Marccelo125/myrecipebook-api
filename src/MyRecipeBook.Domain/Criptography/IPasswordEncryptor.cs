namespace MyRecipeBook.Domain.Criptography
{
    public interface IPasswordEncryptor
    {
        string Encrypt(string password);
        bool ValidPassword(string passwordToValidate, string password);
    }
}
