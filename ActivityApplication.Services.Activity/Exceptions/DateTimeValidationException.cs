namespace ActivityApplication.Services.Activity.Exceptions;

public class DateTimeValidationException : Exception
{
    public DateTimeValidationException(DateTime dateTime) : base($"{dateTime:dd/MM/yyyy} is before today!")
    {
    }
}