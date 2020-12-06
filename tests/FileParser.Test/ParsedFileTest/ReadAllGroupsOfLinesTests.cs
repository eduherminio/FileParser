using System.IO;
using System.Linq;
using Xunit;

namespace FileParser.Test.ParsedFileTest
{
    public class ReadAllGroupsOfLinesTests
    {
        private readonly string _sampleFolderPath = "TestFiles" + Path.DirectorySeparatorChar;

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
    }
}
