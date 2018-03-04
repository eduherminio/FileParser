using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xunit;

using FileParser;

namespace FileParserTest
{
    public class ExtractTests
    {
        [Fact]
        public void CustomLineParse()
        {
            string fileName = "CustomLineParse.txt";
            string sampleContent = "  3  1154 508 100    vegetable ";

            List<long> expectedList = new List<long>() { 1154, 508, 100 };
            string expectedString = "vegetable";

            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                writer.WriteLine(sampleContent);
            }

            IParsedLine line = new ParsedFile(fileName).NextLine();

            int n_ints = line.NextElement<int>();

            for (int i = 0; i < n_ints; ++i)
            {
                long data = line.NextElement<long>();
                Assert.Equal(expectedList.ElementAt(i), data);
            }

            string rawString = line.PeekNextElement<string>();    // Raw string
            string lastString = line.NextElement<string>(); // Converted string => string
            Assert.Equal(rawString, lastString);

            Assert.Equal(expectedString, lastString);

            Assert.True(line.Empty);
        }

        [Fact]
        public void ExtractChar()
        {
            string fileName = "ExtractChar.txt";
            string line1 = "+-*/!?#$%&";
            string line2 = "@()[]{}\"";

            StringBuilder parsedFile = new StringBuilder(string.Empty);

            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                writer.WriteLine(Math.PI.ToString());
                writer.WriteLine(line1);
                writer.WriteLine(line2);
            }

            IParsedFile file = new ParsedFile(fileName);
            IParsedLine firstParsedLine = file.NextLine();
            int nLines = Convert.ToInt32(Math.Floor(firstParsedLine.NextElement<double>())) - 1;
            for (int iLine = 0; iLine < nLines; ++iLine)
            {
                IParsedLine parsedLine = file.NextLine();
                while (parsedLine.Count > 0)
                    parsedFile.Append(parsedLine.NextElement<char>());

                parsedFile.Append('\\');
                Assert.True(parsedLine.Empty);
            }
            Assert.True(file.Empty);

            Assert.Equal(line1 + '\\' + line2 + '\\', parsedFile.ToString());
        }
    }
}
