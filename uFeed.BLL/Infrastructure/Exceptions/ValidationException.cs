namespace uFeed.BLL.Infrastructure.Exceptions
{
    public class ValidationException : ServiceException
    {
        public ValidationException(string message, string property) : base(message, property)
        { }
    }
}
