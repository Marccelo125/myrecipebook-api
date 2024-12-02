namespace MyRecipeBook.Communication.Requests
{
    public class RequestChangePassword
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
