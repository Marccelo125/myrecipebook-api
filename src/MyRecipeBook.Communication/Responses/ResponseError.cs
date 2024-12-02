namespace MyRecipeBook.Communication.Responses
{
    public class ResponseError
    {
        public ResponseError(IList<string> errorMessages)
        {
            ErrorMessages = errorMessages;
        }

        public ResponseError(string errorMessage)
        {
            ErrorMessages = new List<string> { errorMessage };
        }

        public IList<string> ErrorMessages { get; set; }
    }
}
