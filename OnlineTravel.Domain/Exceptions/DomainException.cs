namespace OnlineTravel.Domain.ErrorHandling;

public class DomainException : Exception
{
	public DomainException(string message) : base(message)
	{
	}
}
