namespace FileParser.Utils;

internal static class TypeValidator
{
    /// <summary>
    /// Supported parsing conversions
    /// </summary>
    private static HashSet<Type> SupportedTypes { get; } = new HashSet<Type>()
    {
        typeof(bool),
        typeof(char),
        typeof(string),
        typeof(short),
        typeof(int),
        typeof(long),
        typeof(double),
        typeof(object)
    };

    internal static void ValidateSupportedType<T>()
    {
        if (!SupportedTypes.Contains(typeof(T)))
        {
            throw new NotSupportedException("Parsing to " + typeof(T).ToString() + "is not supported yet");
        }
    }
}
