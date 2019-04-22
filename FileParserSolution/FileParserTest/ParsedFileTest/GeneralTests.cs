using FileParser;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FileParserTest.ParsedFileTest
{
    public class GeneralTests
    {
        private readonly string _sampleFolderPath = "TestFiles" + System.IO.Path.DirectorySeparatorChar;

        [Fact]
        public void BasicTest()
        {
            // Sample file:
            // First line has a random number and a category of aliments.
            // Following lines firstly indicate how many numeric elements are following.
            // After those numeric elements, they include an unknown number of items.

            List<int> numberList = new List<int>();
            List<string> stringList = new List<string>();

            IParsedFile file = new ParsedFile(_sampleFolderPath + "Sample_file.txt");

            IParsedLine firstLine = file.NextLine();

            int n = firstLine.NextElement<int>();
            string str = firstLine.NextElement<string>();

            while (!file.Empty)
            {
                IParsedLine line = file.NextLine();
                int counter = line.NextElement<int>();
                for (int j = 0; j < counter; ++j)
                {
                    numberList.Add(line.NextElement<int>());
                }

                while (!line.Empty)
                {
                    stringList.Add(line.NextElement<string>());
                }
            }

            Assert.Equal(23, n);
            Assert.Equal("food", str);
            Assert.Equal(new List<int>() { 100, 200, 300, 400, 500, 600, 700 }, numberList);
            Assert.Equal(new List<string>() { "apple", "peer", "banana", "meat", "fish" }, stringList);
        }

        [Fact]
        public void PeekTestNotModifiyngFile()
        {
            IParsedFile file = new ParsedFile(_sampleFolderPath + "Sample_file.txt");

            for (int i = 0; i < 10; ++i)
            {
                IParsedLine peekedFirstLine = file.PeekNextLine();       // Allows its modification without extracting it

                int peekedN = peekedFirstLine.PeekNextElement<int>();
                Assert.Equal(peekedN, peekedFirstLine.PeekNextElement<int>());

                IParsedLine peekedFirstLine2 = file.PeekNextLine();

                int peekedN2 = peekedFirstLine.PeekNextElement<int>();
                Assert.Equal(peekedN2, peekedFirstLine2.PeekNextElement<int>());

                Assert.Equal(peekedN, peekedN2);
            }
        }

        [Fact]
        public void PeekTestEmptyingFile()
        {
            IParsedFile file = new ParsedFile(_sampleFolderPath + "Sample_file.txt");

            IParsedLine peekedFirstLine = file.PeekNextLine();   // Allows its modification without extracting it

            int peekedN = peekedFirstLine.PeekNextElement<int>();
            Assert.Equal(peekedN, peekedFirstLine.NextElement<int>());          // Extracting the element
            string peekedStr = peekedFirstLine.PeekNextElement<string>();
            Assert.Equal(peekedStr, peekedFirstLine.NextElement<string>());

            Assert.True(file.NextLine().Empty);

            while (!file.Empty)
            {
                List<int> peekedNumberList = new List<int>();
                List<string> peekedStringList = new List<string>();

                IParsedLine peekedLine = file.PeekNextLine();
                int peekedCounter = peekedLine.PeekNextElement<int>();
                Assert.Equal(peekedCounter, peekedLine.NextElement<int>());
                for (int j = 0; j < peekedCounter; ++j)
                {
                    peekedNumberList.Add(peekedLine.PeekNextElement<int>());
                    Assert.Equal(peekedNumberList.Last(), peekedLine.NextElement<int>());   // Extracting the element
                }

                while (!peekedLine.Empty)
                {
                    peekedStringList.Add(peekedLine.PeekNextElement<string>());
                    Assert.Equal(peekedStringList.Last(), peekedLine.NextElement<string>());   // Extracting the element
                }

                IParsedLine line = file.NextLine();   // Extracting the line, already emptied, to allow the test to finish
                Assert.True(line.Empty);
            }
        }

        [Fact]
        public void PeekTestChangesNotAffectingOriginalFile()
        {
            IParsedFile file = new ParsedFile(_sampleFolderPath + "Sample_file.txt");

            for (int i = 0; i < 10; ++i)
            {
                IParsedLine peekedFirstLine = file.PeekNextLine();       // Allows its modification without extracting it

                int peekedN = peekedFirstLine.PeekNextElement<int>();
                peekedN *= 2;
                Assert.Equal(peekedN, 2 * peekedFirstLine.PeekNextElement<int>());

                IParsedLine peekedFirstLine2 = file.PeekNextLine();

                int peekedN2 = peekedFirstLine.PeekNextElement<int>();
                peekedN2 *= 2;
                Assert.Equal(peekedN2, 2 * peekedFirstLine2.PeekNextElement<int>());

                Assert.Equal(peekedN, peekedN2);
            }
        }

        [Fact]
        public void FileWithEmptyLines()
        {
            IParsedFile file = new ParsedFile(_sampleFolderPath + "FileWithEmtpyLines.txt");
            Assert.True(file.Count == 5);
        }

        [Fact]
        public void LineAt()
        {
            IParsedFile file = new ParsedFile(_sampleFolderPath + "Sample_file.txt");

            IParsedLine lineAt0 = file.LineAt(0);

            int n = lineAt0.PeekNextElement<int>();
            Assert.Equal(23, n);
            int nPlusOne = n + 1;
            lineAt0.Append($" {nPlusOne}");

            file.Append(new ParsedLine(new[] { nPlusOne.ToString() }));
            int totalNumberOfLines = file.Count;

            IParsedLine firstLine = file.NextLine();
            int nAgain = firstLine.NextElement<int>();
            string str = firstLine.NextElement<string>();
            int incrementedN = firstLine.NextElement<int>();
            Assert.Equal(n, nAgain);
            Assert.Equal(nPlusOne, incrementedN);
            Assert.Equal("food", str);

            IParsedLine lastLine = file.LastLine();
            Assert.Equal(incrementedN, lastLine.PeekNextElement<int>());

            for (int lineIndex = 1; lineIndex < totalNumberOfLines - 1; ++lineIndex)
            {
                IParsedLine line = file.NextLine();
                int counter = line.NextElement<int>();
                for (int j = 0; j < counter; ++j)
                {
                    line.NextElement<int>();
                }

                while (!line.Empty)
                {
                    line.NextElement<string>();
                }
            }

            IParsedLine extraLine = file.NextLine();
            Assert.Equal(incrementedN, extraLine.NextElement<int>());
            Assert.True(extraLine.Empty);
            Assert.Throws<ArgumentOutOfRangeException>(() => extraLine.ElementAt<string>(1));
            Assert.Throws<InvalidOperationException>(() => extraLine.LastElement<string>());
            Assert.True(file.Empty);
            Assert.Throws<ArgumentOutOfRangeException>(() => file.LineAt(1));
            Assert.Throws<InvalidOperationException>(() => file.LastLine());
        }
    }
}
