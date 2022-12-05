using Xunit;

namespace FileParser.Test.ParsedFileTest
{
    public class ReadAllGroupsOfLinesTests
    {
        private readonly string _sampleFolderPath = "TestFiles" + Path.DirectorySeparatorChar;

        /// <summary>
        /// Test case taken from https://github.com/eduherminio/AoC2020/blob/main/src/AoC_2020/Day_06.cs
        /// </summary>
        [Fact]
        public void ReadAllGroupsOfLines()
        {
            var parsed = ParsedFile.ReadAllGroupsOfLines(Path.Combine(_sampleFolderPath, $"{nameof(ReadAllGroupsOfLines)}.txt"));

            Assert.Equal(498, parsed.Count);

            Assert.Equal(6662,
                parsed.Sum(group => group.SelectMany(str => str).Distinct().Count()));

            Assert.Equal(3382,
                parsed.Sum(group =>
                {
                    return group
                     .SelectMany(str => str).Distinct()
                     .Count(ch => group.All(str => str.Contains(ch)));
                }));
        }

        /// <summary>
        /// Test case taken from https://github.com/eduherminio/AoC2020/blob/main/src/AoC_2020/Day_06.cs
        /// </summary>
        [Fact]
        public void ReadAllGroupsOfLines_Emptyfile()
        {
            const string filename = nameof(ReadAllGroupsOfLines_Emptyfile);
            string fileContent = Environment.NewLine + Environment.NewLine + "\r\n" + "\n";

            using (StreamWriter writer = new(filename))
            {
                writer.WriteLine(fileContent);
            }
            var parsed = ParsedFile.ReadAllGroupsOfLines(filename);

            Assert.Empty(parsed);
        }
    }
}
