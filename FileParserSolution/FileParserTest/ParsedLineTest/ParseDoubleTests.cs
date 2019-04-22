using FileParser;
using System.IO;
using Xunit;

namespace FileParserTest.ParsedLineTest
{
    public class ParseDoubleTests
    {
        private readonly string _sampleFolderPath = "TestFiles" + Path.DirectorySeparatorChar;

        [Fact]
        public void ParsedDoubleWithCommas()
        {
            IParsedFile parsedFile = new ParsedFile(Path.Combine(_sampleFolderPath, "DoubleWithCommas.txt"));

            ValidateSimpleFile(parsedFile.NextLine(), parsedFile.NextLine());

            Assert.True(parsedFile.Empty);
        }

        [Fact]
        public void ParsedDoubleWithCommasAndPointsAsThousandSeparators()
        {
            IParsedFile parsedFile = new ParsedFile(Path.Combine(_sampleFolderPath, "DoubleWithCommansAndPointsAsThousandsSeparators.txt"));

            ValidateFileWithPointsAndCommas(parsedFile.NextLine(), parsedFile.NextLine(), parsedFile.NextLine(), parsedFile.NextLine());

            Assert.True(parsedFile.Empty);
        }

        [Fact]
        public void ParsedDoubleWithPoints()
        {
            IParsedFile parsedFile = new ParsedFile(Path.Combine(_sampleFolderPath, "DoubleWithPoints.txt"));

            ValidateSimpleFile(parsedFile.NextLine(), parsedFile.NextLine());

            Assert.True(parsedFile.Empty);
        }

        [Fact]
        public void ParsedDoubleWithPointsAndCommasAsThousandSeparators()
        {
            IParsedFile parsedFile = new ParsedFile(Path.Combine(_sampleFolderPath, "DoubleWithPointsAndCommasAsThousandsSeparators.txt"));

            ValidateFileWithPointsAndCommas(parsedFile.NextLine(), parsedFile.NextLine(), parsedFile.NextLine(), parsedFile.NextLine());

            Assert.True(parsedFile.Empty);
        }

        private static void ValidateSimpleFile(IParsedLine firstLine, IParsedLine secondLine)
        {
            Assert.Equal(1.1, firstLine.NextElement<double>());
            Assert.Equal(2.2, firstLine.NextElement<double>());
            Assert.Equal(3.3, firstLine.NextElement<double>());
            Assert.Equal(4.4, firstLine.NextElement<double>());
            Assert.Equal(5.5, firstLine.NextElement<double>());
            Assert.True(firstLine.Empty);

            Assert.Equal(6.6, secondLine.NextElement<double>());
            Assert.Equal(7.7, secondLine.NextElement<double>());
            Assert.Equal(8.8, secondLine.NextElement<double>());
            Assert.Equal(9.9, secondLine.NextElement<double>());
            Assert.Equal(10.10, secondLine.NextElement<double>());
            Assert.True(secondLine.Empty);
        }

        private static void ValidateFileWithPointsAndCommas(IParsedLine firstLine, IParsedLine secondLine, IParsedLine thirdLine, IParsedLine fourthLine)
        {
            Assert.Equal(1_000_000_000, firstLine.NextElement<double>());
            Assert.True(firstLine.Empty);

            Assert.Equal(1_000_000_000.99, secondLine.NextElement<double>());
            Assert.True(secondLine.Empty);

            Assert.Equal(5_000_111.3, thirdLine.NextElement<double>());
            Assert.True(thirdLine.Empty);

            Assert.Equal(0.99_000_111_88, fourthLine.NextElement<double>());
            Assert.True(fourthLine.Empty);
        }
    }
}
