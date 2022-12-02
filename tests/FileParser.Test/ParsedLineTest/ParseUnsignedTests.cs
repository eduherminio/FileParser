using Xunit;

namespace FileParser.Test.ParsedLineTest
{
    public class ParseUnsignedTests
    {
        private readonly string _sampleFolderPath = "TestFiles" + Path.DirectorySeparatorChar;

        [Fact]
        public void ParsedDoubleWithCommas()
        {
            IParsedFile parsedFile = new ParsedFile(Path.Combine(_sampleFolderPath, "Unsigned.txt"));

            ValidateUnsignedFile(parsedFile.NextLine(), parsedFile.NextLine());

            Assert.True(parsedFile.Empty);
        }

        private static void ValidateUnsignedFile(IParsedLine firstLine, IParsedLine secondLine)
        {
            Assert.Equal(0, firstLine.PeekNextElement<ushort>());
            Assert.Equal(0u, firstLine.PeekNextElement<uint>());
            Assert.Equal(0ul, firstLine.NextElement<ulong>());
            Assert.Equal(1234, firstLine.PeekNextElement<ushort>());
            Assert.Equal(1234u, firstLine.PeekNextElement<uint>());
            Assert.Equal(1234ul, firstLine.NextElement<ulong>());
            Assert.Equal(2147483648, firstLine.PeekNextElement<uint>());
            Assert.Equal(2147483648, firstLine.NextElement<ulong>());
            Assert.True(firstLine.Empty);

            Assert.Throws<ArgumentException>(() => secondLine.PeekNextElement<ushort>());
            Assert.Throws<ArgumentException>(() => secondLine.PeekNextElement<uint>());
            Assert.Throws<ArgumentException>(() => secondLine.PeekNextElement<ulong>());
        }
    }
}
