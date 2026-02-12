namespace OnlineTravel.Domain.ErrorHandling
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
