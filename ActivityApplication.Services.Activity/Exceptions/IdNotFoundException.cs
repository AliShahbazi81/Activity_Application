namespace ActivityApplication.Services.Activity.Exceptions;

public class IdNotFoundException : Exception
{
    public IdNotFoundException() : base("Activity record not found!")
    {
    }
}