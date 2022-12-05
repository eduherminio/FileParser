/*
*
*   |                                Method |                 path |     Mean |    Error |   StdDev |   Median | Ratio | RatioSD |    Gen0 |    Gen1 |   Gen2 | Allocated | Alloc Ratio |
*   |-------------------------------------- |--------------------- |---------:|---------:|---------:|---------:|------:|--------:|--------:|--------:|-------:|----------:|------------:|
*   |  ReadAllGroupsOfLinesGeneric_Original | Bench(...)6.txt [49] | 661.5 us | 18.13 us | 50.83 us | 646.8 us |  1.00 |    0.00 | 83.9844 | 29.2969 |      - |  325.8 KB |        1.00 |
*   | ReadAllGroupsOfLinesGeneric_Optimized | Bench(...)6.txt [49] | 352.0 us |  7.01 us | 10.92 us | 352.2 us |  0.53 |    0.05 | 54.1992 | 26.8555 | 0.4883 | 237.05 KB |        0.73 |
*   |                                       |                      |          |          |          |          |       |         |         |         |        |           |             |
*   |  ReadAllGroupsOfLinesGeneric_Original | Bench(...)1.txt [49] | 452.0 us |  8.98 us | 11.68 us | 452.1 us |  1.00 |    0.00 | 94.2383 | 31.7383 | 0.4883 | 235.73 KB |        1.00 |
*   | ReadAllGroupsOfLinesGeneric_Optimized | Bench(...)1.txt [49] | 303.9 us |  5.29 us |  8.98 us | 304.0 us |  0.67 |    0.03 | 52.7344 | 24.9023 | 0.4883 | 226.63 KB |        0.96 |
*   |                                       |                      |          |          |          |          |       |         |         |         |        |           |             |
*   |  ReadAllGroupsOfLinesGeneric_Original | Bench(...)t.txt [62] | 105.3 us |  1.87 us |  1.92 us | 105.7 us |  1.00 |    0.00 |  6.1035 |  3.0518 |      - |  12.59 KB |        1.00 |
*   | ReadAllGroupsOfLinesGeneric_Optimized | Bench(...)t.txt [62] | 106.2 us |  2.10 us |  2.50 us | 106.9 us |  1.01 |    0.03 |  5.8594 |  2.9297 |      - |  12.02 KB |        0.96 |
 */

using BenchmarkDotNet.Attributes;
using FileParser;

namespace FileParser.Benchmark;

[MemoryDiagnoser]
public class ReadAllGroupsOfLines_
{
    public static IEnumerable<string> Data => new[] {
        "BenchmarkFiles/ReadAllGroupsOfLines_AoC2020_6.txt",
        "BenchmarkFiles/ReadAllGroupsOfLines_AoC2022_1.txt",
        "BenchmarkFiles/ReadAllGroupsOfLines_AoC2022_1_small_subset.txt"
    };

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(Data))]
    public List<List<string>> ReadAllGroupsOfLinesGeneric_Original(string path) => ReadAllGroupsOfLines_Original(path);

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public List<List<string>> ReadAllGroupsOfLinesGeneric_Optimized(string path) => ParsedFile.ReadAllGroupsOfLines(path);

    public static List<List<string>> ReadAllGroupsOfLines_Original(string path)
    {
        return File.ReadAllText(path)
            .Replace("\r\n", "\n")
            .Split("\n\n")
            .Where(str => !string.IsNullOrEmpty(str))
            .Select(str => str.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList())
            .ToList();
    }
}
