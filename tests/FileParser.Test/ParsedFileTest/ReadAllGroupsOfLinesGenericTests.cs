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
                ParsedFile.ReadAllGroupsOfLines<char>(Path.Combine(_sampleFolderPath, $"{nameof(ReadAllGroupsOfLinesGenericInt)}.txt")));
        }

        [Fact]
        public void ReadAllGroupsOfLinesGenericString()
        {
            Assert.Throws<NotSupportedException>(() =>
                ParsedFile.ReadAllGroupsOfLines<string>(Path.Combine(_sampleFolderPath, $"{nameof(ReadAllGroupsOfLinesGenericInt)}.txt")));
        }

        /// <summary>
        /// Test case taken from https://github.com/eduherminio/AoC2022/blob/main/src/AoC_2022/Day_01.cs
        /// </summary>
        [Fact]
        public void ReadAllGroupsOfLinesGenericInt()
        {
            var parsed = ParsedFile.ReadAllGroupsOfLines<int>(Path.Combine(_sampleFolderPath, $"{nameof(ReadAllGroupsOfLinesGenericInt)}.txt"));

            Assert.Equal(66186, parsed.Max(n => n.Sum()));
            Assert.Equal(196804, parsed.Select(n => n.Sum()).OrderByDescending(n => n).Take(3).Sum());
        }

        [Fact]
        public void ReadAllGroupsOfLinesGenericEmpty()
        {
            const string filename = nameof(ReadAllGroupsOfLinesGenericEmpty);
            string fileContent = Environment.NewLine + Environment.NewLine + "\r\n" + "\n";

            using (StreamWriter writer = new(filename))
            {
                writer.WriteLine(fileContent);
            }
            Assert.Empty(ParsedFile.ReadAllGroupsOfLines<long>(filename));
            Assert.Empty(ParsedFile.ReadAllGroupsOfLines<int>(filename));
            Assert.Empty(ParsedFile.ReadAllGroupsOfLines<ulong>(filename));
        }
    }
}
