/*
*
*   |                                        Method |                 path |     Mean |    Error |   StdDev | Ratio | RatioSD |    Gen0 |    Gen1 |   Gen2 | Allocated | Alloc Ratio |
*   |---------------------------------------------- |--------------------- |---------:|---------:|---------:|------:|--------:|--------:|--------:|-------:|----------:|------------:|
*   |          ReadAllGroupsOfLinesGeneric_Original | Bench(...)6.txt [49] | 613.6 us | 12.19 us | 23.20 us |  1.00 |    0.00 | 82.0313 | 36.1328 |      - |  325.8 KB |        1.00 |
*   | ReadAllGroupsOfLinesGeneric_FirstOptimization | Bench(...)6.txt [49] | 337.8 us |  6.70 us |  7.71 us |  0.55 |    0.02 | 51.2695 | 25.3906 | 0.4883 | 237.05 KB |        0.73 |
*   |         ReadAllGroupsOfLinesGeneric_Optimized | Bench(...)6.txt [49] | 298.5 us |  5.92 us | 10.52 us |  0.49 |    0.02 | 32.2266 | 16.1133 | 0.4883 | 145.57 KB |        0.45 |
*
*   |          ReadAllGroupsOfLinesGeneric_Original | Bench(...)1.txt [49] | 439.5 us |  8.46 us | 10.39 us |  1.00 |    0.00 | 94.2383 | 31.7383 | 0.4883 | 235.73 KB |        1.00 |
*   | ReadAllGroupsOfLinesGeneric_FirstOptimization | Bench(...)1.txt [49] | 299.5 us |  5.57 us | 10.45 us |  0.68 |    0.03 | 53.7109 | 25.3906 | 0.4883 | 226.63 KB |        0.96 |
*   |         ReadAllGroupsOfLinesGeneric_Optimized | Bench(...)1.txt [49] | 252.5 us |  4.99 us |  8.06 us |  0.58 |    0.02 | 36.1328 | 17.0898 | 0.4883 | 135.09 KB |        0.57 |
*
*   |          ReadAllGroupsOfLinesGeneric_Original | Bench(...)t.txt [62] | 103.3 us |  2.05 us |  2.01 us |  1.00 |    0.00 |  6.1035 |  3.0518 |      - |  12.59 KB |        1.00 |
*   | ReadAllGroupsOfLinesGeneric_FirstOptimization | Bench(...)t.txt [62] | 104.1 us |  2.02 us |  2.33 us |  1.01 |    0.03 |  5.8594 |  2.9297 |      - |  12.02 KB |        0.96 |
*   |         ReadAllGroupsOfLinesGeneric_Optimized | Bench(...)t.txt [62] | 103.7 us |  1.75 us |  1.55 us |  1.00 |    0.03 |  5.0049 |  2.4414 |      - |  10.32 KB |        0.82 |
 */

using BenchmarkDotNet.Attributes;

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
    public List<List<string>> ReadAllGroupsOfLinesGeneric_FirstOptimization(string path) => ReadAllGroupsOfLines_FirstOptimization(path);

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public List<List<string>> ReadAllGroupsOfLinesGeneric_Optimized(string path) => ParsedFile.ReadAllGroupsOfLines(path);

    /// <summary>
    /// v2.3.0
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static List<List<string>> ReadAllGroupsOfLines_Original(string path)
    {
        return File.ReadAllText(path)
            .Replace("\r\n", "\n")
            .Split("\n\n")
            .Where(str => !string.IsNullOrEmpty(str))
            .Select(str => str.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList())
            .ToList();
    }

    /// <summary>
    /// File.ReadAllLines(path) instead of File.ReadAllText(path)
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static List<List<string>> ReadAllGroupsOfLines_FirstOptimization(string path)
    {
        var allLines = File.ReadAllLines(path);

        var currentGroup = new List<string>();
        var result = new List<List<string>>(allLines.Length)
        {
            currentGroup
        };

        foreach (var line in allLines)
        {
            if (line.Length == 0)
            {
                if (currentGroup.Count != 0)
                {
                    currentGroup = new List<string>();
                    result.Add(currentGroup);
                }
            }
            else
            {
                currentGroup.Add(line);
            }
        }

        if (currentGroup.Count == 0)
        {
            result.Remove(currentGroup);
        }

        return result;
    }
}
