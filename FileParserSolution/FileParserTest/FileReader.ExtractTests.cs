using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

using FileParser;

namespace FileParserTest
{
    public class ExtractTests
    {
        [Fact]
        void CustomLineParse()
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

            Queue<string> queue = new Queue<string>(FileReader.ParseLine(fileName));

            int n_ints = FileReader.Extract<int>(ref queue);

            for (int i = 0; i < n_ints; ++i)
            {
                long data = FileReader.Extract<long>(ref queue);
                Assert.Equal(expectedList.ElementAt(i), data);
            }

            string rawString = queue.Peek();    // Raw string
            string lastString = FileReader.Extract<string>(ref queue); // Converted string => string
            Assert.Equal(rawString, lastString);

            Assert.Equal(expectedString, lastString);

            Assert.Empty(queue);
        }

        [Fact]
        void ExtractCharUsingFileReader()
        {
            string fileName = "ExtractChar.txt";
            string line1 = "+-*/!?#$%&";
            string line2 = "@()[]{}\"";

            string parsedFile = string.Empty;

            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                writer.WriteLine(Math.PI.ToString());
                writer.WriteLine(line1);
                writer.WriteLine(line2);
            }

            Queue<Queue<string>> file = FileReader.ParseFile(fileName);
            Queue<string> firstParsedLine = file.Dequeue();
            int nLines = Convert.ToInt32(Math.Floor(FileReader.Extract<double>(ref firstParsedLine))) - 1;
            for (int iLine = 0; iLine < nLines; ++iLine)
            {
                Queue<string> parsedLine = file.Dequeue();
                while (parsedLine.Count > 0)
                    parsedFile += FileReader.Extract<char>(ref parsedLine);

                parsedFile += '\\';
                Assert.Empty(parsedLine);
            }
            Assert.Empty(file);

            Assert.Equal(line1 + '\\' + line2 + '\\', parsedFile);
        }

        [Fact]
        void ExtractCharUsingParsedFile()
        {
            string fileName = "ExtractChar.txt";
            string line1 = "+-*/!?#$%&";
            string line2 = "@()[]{}\"";

            string parsedFile = string.Empty;

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
                    parsedFile += parsedLine.NextElement<char>();

                parsedFile += '\\';
                Assert.True(parsedLine.Empty);
            }
            Assert.True(file.Empty);

            Assert.Equal(line1 + '\\' + line2 + '\\', parsedFile);
        }
    }
}
