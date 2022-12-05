/*
*   |                                         Method |                 path |       Mean |    Error |    StdDev |     Median | Ratio | RatioSD |     Gen0 |    Gen1 | Allocated | Alloc Ratio |
*   |----------------------------------------------- |--------------------- |-----------:|---------:|----------:|-----------:|------:|--------:|---------:|--------:|----------:|------------:|
*   |           ReadAllGroupsOfLinesGeneric_Original | Bench(...)1.txt [49] | 1,489.5 us | 79.92 us | 233.14 us | 1,399.9 us |  1.00 |    0.00 | 175.7813 |       - | 365.95 KB |        1.00 |
*   |  ReadAllGroupsOfLinesGeneric_FirstOptimization | Bench(...)1.txt [49] | 1,299.9 us | 38.98 us | 113.70 us | 1,277.1 us |  0.89 |    0.14 | 123.0469 | 35.1563 |  335.2 KB |        0.92 |
*   | ReadAllGroupsOfLinesGeneric_SecondOptimization | Bench(...)1.txt [49] |   375.1 us | 11.78 us |  33.99 us |   369.5 us |  0.26 |    0.04 |  76.1719 | 25.3906 | 210.07 KB |        0.57 |
*   |          ReadAllGroupsOfLinesGeneric_Optimized | Bench(...)1.txt [49] |   330.0 us | 11.75 us |  34.46 us |   316.0 us |  0.23 |    0.04 |  57.6172 | 17.5781 | 118.53 KB |        0.32 |
*
*   |           ReadAllGroupsOfLinesGeneric_Original | Bench(...)t.txt [62] |   129.2 us |  3.19 us |   9.14 us |   127.1 us |  1.00 |    0.00 |   7.0801 |  3.4180 |  14.75 KB |        1.00 |
*   |  ReadAllGroupsOfLinesGeneric_FirstOptimization | Bench(...)t.txt [62] |   126.3 us |  2.92 us |   7.95 us |   125.1 us |  0.98 |    0.09 |   6.3477 |  3.1738 |  13.45 KB |        0.91 |
*   | ReadAllGroupsOfLinesGeneric_SecondOptimization | Bench(...)t.txt [62] |   108.1 us |  2.06 us |   2.37 us |   108.1 us |  0.86 |    0.04 |   5.7373 |  2.8076 |  11.82 KB |        0.80 |
*   |          ReadAllGroupsOfLinesGeneric_Optimized | Bench(...)t.txt [62] |   106.9 us |  2.12 us |   2.97 us |   106.2 us |  0.84 |    0.05 |   4.8828 |  2.4414 |  10.12 KB |        0.69 |
 */

using BenchmarkDotNet.Attributes;
using FileParser.Utils;

namespace FileParser.Benchmark;

[MemoryDiagnoser]
public class ReadAllGroupsOfLinesGeneric
{
    public static IEnumerable<string> Data => new[] {
        "BenchmarkFiles/ReadAllGroupsOfLines_AoC2022_1.txt",
        "BenchmarkFiles/ReadAllGroupsOfLines_AoC2022_1_small_subset.txt"
    };

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(Data))]
    public List<List<int>> ReadAllGroupsOfLinesGeneric_Original(string path) => ReadAllGroupsOfLines_Original<int>(path);

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public List<List<int>> ReadAllGroupsOfLinesGeneric_FirstOptimization(string path) => ReadAllGroupsOfLines_FirstOptimization<int>(path);

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public List<List<int>> ReadAllGroupsOfLinesGeneric_SecondOptimization(string path) => ReadAllGroupsOfLines_SecondOptimization<int>(path);

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public List<List<int>> ReadAllGroupsOfLinesGeneric_Optimized(string path) => ParsedFile.ReadAllGroupsOfLines<int>(path);

    /// <summary>
    /// v2.3.0
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static List<List<T>> ReadAllGroupsOfLines_Original<T>(string path)
    {
        if (typeof(T) == typeof(char))
        {
            throw new NotSupportedException($"{nameof(ReadAllGroupsOfLines_Original)}<T> does not support T = char, " +
                "use ReadAllGroupsOfLines<string> instead and split the strings yourself");
        }

        if (typeof(T) == typeof(string))
        {
            throw new NotSupportedException($"{nameof(ReadAllGroupsOfLines_Original)}<T> does not support T = string, " +
                "use non-generic ReadAllGroupsOfLines instead");
        }

        return File.ReadAllText(path)
                .Replace("\r\n", "\n")
                .Split("\n\n")
                .Where(str => !string.IsNullOrEmpty(str))
                .Select(str => str
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .Select(str => StringConverter.GenericConvert<T>(str))
                    .ToList())
                .ToList();
    }

    /// <summary>
    /// File.ReadAllLines(path) instead of File.ReadAllText(path)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static List<List<T>> ReadAllGroupsOfLines_FirstOptimization<T>(string path)
    {
        TypeValidator.ValidateSupportedType<T>();

        if (typeof(T) == typeof(char))
        {
            throw new NotSupportedException($"{nameof(ReadAllGroupsOfLines_FirstOptimization)}<T> does not support T = char, " +
                "use ReadAllGroupsOfLines<string> instead and split the strings yourself");
        }

        if (typeof(T) == typeof(string))
        {
            throw new NotSupportedException($"{nameof(ReadAllGroupsOfLines_FirstOptimization)}<T> does not support T = string, " +
                "use non-generic ReadAllGroupsOfLines instead");
        }

        var allLines = File.ReadAllLines(path);

        var currentGroup = new List<T>();
        var result = new List<List<T>>(allLines.Length)
            {
                currentGroup
            };

        foreach (var line in allLines)
        {
            if (line.Length == 0)
            {
                if (currentGroup.Count != 0)
                {
                    currentGroup = new List<T>();
                    result.Add(currentGroup);
                }
            }
            else
            {
                currentGroup.Add(StringConverter.GenericConvert<T>(line));
            }
        }

        if (currentGroup.Count == 0)
        {
            result.Remove(currentGroup);
        }

        return result;
    }

    /// <summary>
    /// StringConverter.Convert<T>() instead of StringConverter.GenericConvert<T>()
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static List<List<T>> ReadAllGroupsOfLines_SecondOptimization<T>(string path)
    {
        TypeValidator.ValidateSupportedType<T>();

        if (typeof(T) == typeof(char))
        {
            throw new NotSupportedException($"{nameof(ReadAllGroupsOfLines_SecondOptimization)}<T> does not support T = char, " +
                "use ReadAllGroupsOfLines<string> instead and split the strings yourself");
        }

        if (typeof(T) == typeof(string))
        {
            throw new NotSupportedException($"{nameof(ReadAllGroupsOfLines_SecondOptimization)}<T> does not support T = string, " +
                "use non-generic ReadAllGroupsOfLines instead");
        }

        var allLines = File.ReadAllLines(path);

        var currentGroup = new List<T>();
        var result = new List<List<T>>(allLines.Length)
        {
            currentGroup
        };

        foreach (var line in allLines)
        {
            if (line.Length == 0)
            {
                if (currentGroup.Count != 0)
                {
                    currentGroup = new List<T>();
                    result.Add(currentGroup);
                }
            }
            else
            {
                currentGroup.Add(StringConverter.Convert<T>(line));
            }
        }

        if (currentGroup.Count == 0)
        {
            result.Remove(currentGroup);
        }

        return result;
    }
}
