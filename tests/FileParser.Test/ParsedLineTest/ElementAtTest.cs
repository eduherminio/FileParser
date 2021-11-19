using Xunit;

namespace FileParser.Test.ParsedLineTest
{
    public class ElementAtTest
    {
        private readonly string _sampleFolderPath = "TestFiles" + Path.DirectorySeparatorChar;

        [Fact]
        public void ElementAt()
        {
            IParsedFile file = new ParsedFile(_sampleFolderPath + "Sample_file.txt");

            IParsedLine firstLine = file.NextLine();
            string lastElement = firstLine.ElementAt<string>(firstLine.Count - 1);
            Assert.Equal("food", lastElement);

            IParsedLine peekedSecondLine = file.PeekNextLine();
            int number = peekedSecondLine.ElementAt<int>(1);
            Assert.Equal(100, number);
            const double modifiedNumber = 100 + Math.PI;
            peekedSecondLine.Append($" {modifiedNumber}");

            IParsedLine secondLine = file.NextLine();
            int numberAgain = secondLine.ElementAt<int>(1);
            Assert.Equal(number, numberAgain);

            double addedNumber = secondLine.ElementAt<double>(secondLine.Count - 1);
            Assert.Equal(modifiedNumber, addedNumber, 10);
        }
    }
}
