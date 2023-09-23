namespace ActivityApplication.Services.Activity.Exceptions;

public class StringLengthException : Exception
{
    public StringLengthException(string text, int length) : base($"The length of {text}  must be at least {length}!")
    {
    }
}