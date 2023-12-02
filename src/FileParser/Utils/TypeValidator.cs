namespace FileParser.Utils;

internal static class TypeValidator
{
    /// <summary>
    /// Supported parsing conversions
    /// </summary>
    private static HashSet<Type> SupportedTypes { get; } =
    [
        typeof(bool),
        typeof(char),
        typeof(string),
        typeof(ushort),
        typeof(short),
        typeof(int),
        typeof(uint),
        typeof(ulong),
        typeof(long),
        typeof(double),
        typeof(object)
    ];

    internal static void ValidateSupportedType<T>()
    {
        if (!SupportedTypes.Contains(typeof(T)))
        {
            throw new NotSupportedException("Parsing to " + typeof(T) + "is not supported yet");
        }
    }
}
