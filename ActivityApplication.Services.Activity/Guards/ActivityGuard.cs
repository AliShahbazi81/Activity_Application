using ActivityApplication.Services.Activity.Exceptions;

namespace ActivityApplication.Services.Activity.Guards;

public static class ActivityGuard
{
    // Check if selected date is less than today
    public static void CheckDate(DateTime date)
    {
        if (date < DateTime.UtcNow)
            throw new DateTimeValidationException(date);
    }

    // Check if the length of a string is less than x
    public static void CheckLength(string text, int length)
    {
        if (text.Length < 5)
            throw new StringLengthException(text, length);
    }
}