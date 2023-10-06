namespace ActivityApplication.Domain.ExceptionsHandling;

public class AppException
{
    public AppException(int statusCode, string message, string? details = null)
    {
        StatusCode = statusCode;
        Message = message;
        Details = details;
    }

    public int StatusCode { get; set; }

    // Error Message
    public string Message { get; set; }

    // Stack Trace
    public string Details { get; set; }
}