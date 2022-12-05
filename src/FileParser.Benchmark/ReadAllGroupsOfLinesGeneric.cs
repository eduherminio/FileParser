/*
*
*   |                                         Method |                 path |       Mean |    Error |    StdDev |     Median | Ratio | RatioSD |     Gen0 |    Gen1 | Allocated | Alloc Ratio |
*   |----------------------------------------------- |--------------------- |-----------:|---------:|----------:|-----------:|------:|--------:|---------:|--------:|----------:|------------:|
*   |        ReadAllGroupsOfLinesGeneric_OriginalInt | Bench(...)1.txt [49] | 1,756.9 us | 85.87 us | 253.20 us | 1,802.3 us |  1.00 |    0.00 | 175.7813 |       - | 366.07 KB |        1.00 |
*   |       ReadAllGroupsOfLinesGeneric_OptimizedInt | Bench(...)1.txt [49] | 1,395.7 us | 81.46 us | 236.32 us | 1,293.7 us |  0.80 |    0.12 | 126.9531 | 35.1563 |  335.2 KB |        0.92 |
*   | ReadAllGroupsOfLinesGeneric_DoubleOptimizedInt | Bench(...)1.txt [49] |   335.8 us |  6.54 us |  10.37 us |   337.0 us |  0.17 |    0.01 |  78.6133 | 26.3672 | 210.07 KB |        0.57 |
*
*   |        ReadAllGroupsOfLinesGeneric_OriginalInt | Bench(...)t.txt [62] |   119.5 us |  2.28 us |   3.99 us |   119.8 us |  1.00 |    0.00 |   7.0801 |  3.4180 |  14.75 KB |        1.00 |
*   |       ReadAllGroupsOfLinesGeneric_OptimizedInt | Bench(...)t.txt [62] |   118.3 us |  2.23 us |   2.57 us |   118.6 us |  0.98 |    0.05 |   6.4697 |  3.1738 |  13.45 KB |        0.91 |
*   | ReadAllGroupsOfLinesGeneric_DoubleOptimizedInt | Bench(...)t.txt [62] |   105.3 us |  2.08 us |   2.92 us |   104.6 us |  0.88 |    0.04 |   5.7373 |  2.8076 |  11.82 KB |        0.80 |
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
    public List<List<int>> ReadAllGroupsOfLinesGeneric_Optimized(string path) => ParsedFile.ReadAllGroupsOfLines<int>(path);

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
}
