namespace Common;

public static class NullExtensionMethods
{
    public static bool IsNullOrEmpty(this string value)
        => string.IsNullOrEmpty(value);

    public static bool IsNullOrWhitespace(this string value)
        => string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Checks if the value of a string is null or empty and throws ArgumentException if so.
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="ArgumentException"></exception>
    public static void AssertIsNotNullOrEmpty(this string value)
    {
        if (value.IsNullOrEmpty())
            throw new ArgumentException("The passed string value is null or empty.", nameof(value));
    }

    /// <summary>
    /// Checks if the value of a string is null or whitespace and throws ArgumentException if so.
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="ArgumentException"></exception>
    public static void AssertIsNotNullOrWhitespace(this string value)
    {
        if (value.IsNullOrEmpty())
            throw new ArgumentException("The passed string value is null or whitespace.", nameof(value));
    }

    public static void AssertIsNotNull(this object value)
    {
        if (value == null)
            throw new ArgumentException("The passed object is null.", nameof(value));
    }
}
