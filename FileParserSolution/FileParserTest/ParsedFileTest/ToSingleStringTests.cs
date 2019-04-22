using FileParser;
using System.IO;
using Xunit;

namespace FileParserTest.ParsedFileTest
{
    public class ToSingleStringTests
    {
        private readonly string _sampleFolderPath = "TestFiles" + Path.DirectorySeparatorChar;

        [Fact]
        public void ToSingleStringWithDefaultSeparators()
        {
            IParsedFile parsedFile = new ParsedFile(Path.Combine(_sampleFolderPath, "ToSingleString.txt"));

            Assert.Equal(
                "0 This 1234 is a line with some lines of text 404 Second line 1 2 34 end",
                parsedFile.ToSingleString());

            Assert.True(parsedFile.Empty);
        }

        [Fact]
        public void ToSingleStringWithEmptyWordSeparator()
        {
            IParsedFile parsedFile = new ParsedFile(Path.Combine(_sampleFolderPath, "ToSingleString.txt"));

            Assert.Equal(
                "0This1234isalinewithsomelinesoftext404Secondline1234end",
                parsedFile.ToSingleString(wordSeparator: string.Empty));

            Assert.True(parsedFile.Empty);
        }

        [Fact]
        public void ToSingleStringWithEmptyWordSeparatorAndCustomLineSeparator()
        {
            IParsedFile parsedFile = new ParsedFile(Path.Combine(_sampleFolderPath, "ToSingleString.txt"));

            Assert.Equal(
                "0This1234isalinewithsomelinesoftext404\nSecondline1234\nend",
                parsedFile.ToSingleString(wordSeparator: string.Empty, lineSeparator: "\n"));

            Assert.True(parsedFile.Empty);
        }
    }
}
