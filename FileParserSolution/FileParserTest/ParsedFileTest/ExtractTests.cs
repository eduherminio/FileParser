using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xunit;

using FileParser;

namespace FileParserTest.ParsedFileTest
{
    public class ExtractTests
    {
        [Fact]
        public void CustomLineParse()
        {
            const string fileName = "CustomLineParse.txt";
            const string sampleContent = "  3  1154 508 100    vegetable ";

            List<long> expectedList = new List<long>() { 1154, 508, 100 };
            const string expectedString = "vegetable";

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine(sampleContent);
            }

            IParsedLine line = new ParsedFile(fileName).NextLine();

            int n_ints = line.NextElement<int>();

            for (int i = 0; i < n_ints; ++i)
            {
                long data = line.NextElement<long>();
                Assert.Equal(expectedList[i], data);
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
            const string fileName = "ExtractChar.txt";
            const string line1 = "+-*/!?#$%&";
            const string line2 = "@()[]{}\"";

            StringBuilder parsedFile = new StringBuilder(string.Empty);

            using (StreamWriter writer = new StreamWriter(fileName))
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
                while (!parsedLine.Empty)
                {
                    parsedFile.Append(parsedLine.NextElement<char>());
                }

                parsedFile.Append('\\');
                Assert.True(parsedLine.Empty);
            }
            Assert.True(file.Empty);

            Assert.Equal(line1 + '\\' + line2 + '\\', parsedFile.ToString());
        }
    }
}
