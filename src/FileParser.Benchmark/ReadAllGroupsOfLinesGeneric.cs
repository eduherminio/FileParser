/*
*
*   |                                   Method |                 path |       Mean |    Error |   StdDev | Ratio | RatioSD |     Gen0 |    Gen1 | Allocated | Alloc Ratio |
*   |----------------------------------------- |--------------------- |-----------:|---------:|---------:|------:|--------:|---------:|--------:|----------:|------------:|
*   |  ReadAllGroupsOfLinesGeneric_OriginalInt | Bench(...)1.txt [49] | 1,336.4 us | 27.90 us | 77.31 us |  1.00 |    0.00 | 177.7344 |       - | 365.95 KB |        1.00 |
*   | ReadAllGroupsOfLinesGeneric_OptimizedInt | Bench(...)1.txt [49] | 1,202.0 us | 23.76 us | 61.33 us |  0.90 |    0.07 | 126.9531 | 37.1094 |  335.2 KB |        0.92 |
*   |                                          |                      |            |          |          |       |         |          |         |           |             |
*   |  ReadAllGroupsOfLinesGeneric_OriginalInt | Bench(...)t.txt [62] |   116.8 us |  2.23 us |  2.09 us |  1.00 |    0.00 |   7.2021 |  3.5400 |  14.75 KB |        1.00 |
*   | ReadAllGroupsOfLinesGeneric_OptimizedInt | Bench(...)t.txt [62] |   117.8 us |  2.31 us |  2.16 us |  1.01 |    0.02 |   6.4697 |  3.1738 |  13.45 KB |        0.91 |
 */

using BenchmarkDotNet.Attributes;

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
    public List<List<int>> ReadAllGroupsOfLinesGeneric_OriginalInt(string path) => ReadAllGroupsOfLinesGeneric_Original<int>(path);

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public List<List<int>> ReadAllGroupsOfLinesGeneric_OptimizedInt(string path) => ParsedFile.ReadAllGroupsOfLines<int>(path);

    public static List<List<T>> ReadAllGroupsOfLinesGeneric_Original<T>(string path)
    {
        if (typeof(T) == typeof(char))
        {
            throw new NotSupportedException($"{nameof(ReadAllGroupsOfLinesGeneric_Original)}<T> does not support T = char, " +
                "use ReadAllGroupsOfLines<string> instead and split the strings yourself");
        }

        if (typeof(T) == typeof(string))
        {
            throw new NotSupportedException($"{nameof(ReadAllGroupsOfLinesGeneric_Original)}<T> does not support T = string, " +
                "use non-generic ReadAllGroupsOfLines instead");
        }

        return File.ReadAllText(path)
                .Replace("\r\n", "\n")
                .Split("\n\n")
                .Where(str => !string.IsNullOrEmpty(str))
                .Select(str => str
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .Select(str => StringConverter.Convert<T>(str))
                    .ToList())
                .ToList();
    }
}
