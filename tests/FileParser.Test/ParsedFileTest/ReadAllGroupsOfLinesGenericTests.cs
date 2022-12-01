using Xunit;

namespace FileParser.Test.ParsedFileTest
{
    public class ReadAllGroupsOfLinesGenericTests
    {
        private readonly string _sampleFolderPath = "TestFiles" + Path.DirectorySeparatorChar;

        [Fact]
        public void ReadAllGroupsOfLinesGenericChar()
        {
            Assert.Throws<NotSupportedException>(() =>
                ParsedFile.ReadAllGroupsOfLines<char>(Path.Combine(_sampleFolderPath, $"{nameof(ReadAllGroupsOfLinesInt)}.txt")));
        }

        [Fact]
        public void ReadAllGroupsOfLinesGenericString()
        {
            Assert.Throws<NotSupportedException>(() =>
                ParsedFile.ReadAllGroupsOfLines<string>(Path.Combine(_sampleFolderPath, $"{nameof(ReadAllGroupsOfLinesInt)}.txt")));
        }

        /// <summary>
        /// Test case taken from https://github.com/eduherminio/AoC2022/blob/main/src/AoC_2022/Day_01.cs
        /// </summary>
        [Fact]
        public void ReadAllGroupsOfLinesInt()
        {
            var parsed = ParsedFile.ReadAllGroupsOfLines<int>(Path.Combine(_sampleFolderPath, $"{nameof(ReadAllGroupsOfLinesInt)}.txt"));

            Assert.Equal(66186, parsed.Max(n => n.Sum()));
            Assert.Equal(196804, parsed.Select(n => n.Sum()).OrderByDescending(n => n).Take(3).Sum());
        }
    }
}
