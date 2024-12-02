using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionsBase
{
    public class NotOwnerException : MyRecipeBookException
    {
        public NotOwnerException(string message) : base(message) { }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Forbidden;
    }
}
