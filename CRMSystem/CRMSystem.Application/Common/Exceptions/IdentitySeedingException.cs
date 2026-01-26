namespace CRMSystem.Application.Common.Exceptions;

public class IdentitySeedingException: Exception
{
    public IdentitySeedingException(string message) : base(message)
    {
    }   
    
    public IdentitySeedingException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}