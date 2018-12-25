using FileParser;
using System.IO;
using Xunit;

namespace FileParserTest.ParsedLineTest
{
    public class ToSingleStringTests
    {
        private readonly string _sampleFolderPath = "TestFiles" + Path.DirectorySeparatorChar;

        [Fact]
        public void ToSingleStringWithDefaultSeparator()
        {
            IParsedFile parsedFile = new ParsedFile(Path.Combine(_sampleFolderPath, "ToSingleString.txt"));

            IParsedLine firstLine = parsedFile.NextLine();

            Assert.Equal(0, firstLine.NextElement<int>());
            Assert.Equal("This", firstLine.NextElement<string>());

            Assert.Equal(
                "1234 is a line with some lines of text 404",
                firstLine.ToSingleString());

            Assert.True(firstLine.Empty);
        }

        [Fact]
        public void ToSingleStringWithCustomSeparator()
        {
            IParsedFile parsedFile = new ParsedFile(Path.Combine(_sampleFolderPath, "ToSingleString.txt"));

            parsedFile.NextLine();
            IParsedLine secondLine = parsedFile.NextLine();

            Assert.Equal(
                "Second^~line^~1^~2^~34",
                secondLine.ToSingleString("^~"));

            Assert.True(secondLine.Empty);
        }

        [Fact]
        public void ToSingleStringWithEmptySeparator()
        {
            IParsedFile parsedFile = new ParsedFile(Path.Combine(_sampleFolderPath, "ToSingleString.txt"));

            IParsedLine firstLine = parsedFile.NextLine();

            Assert.Equal(
                "0This1234isalinewithsomelinesoftext404",
                firstLine.ToSingleString(string.Empty));

            Assert.True(firstLine.Empty);
        }
    }
}
