using FileParser;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace FileParserTest
{
    public class ExceptionTests
    {
        private readonly string _validPath = "TestFiles" + Path.DirectorySeparatorChar;

        [Fact]
        public void NotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => new ParsedFile(_validPath + "Sample_file.txt").ToList<uint>());
            Assert.Throws<NotSupportedException>(() => new ParsedFile(_validPath + "Sample_file.txt").ToList<DateTime>());
            var line = new ParsedLine(new Queue<string>(new string[] { "1234" }));
            Assert.Throws<NotSupportedException>(() => line.NextElement<ulong>());

            IParsedFile parsedFile = new ParsedFile(_validPath + "Sample_file.txt");
            IParsedLine firstParsedLine = parsedFile.NextLine();

            Assert.Throws<NotSupportedException>(() => firstParsedLine.NextElement<char>());
        }

        [Fact]
        public void FileNotFoundException()
        {
            Assert.Throws<FileNotFoundException>(() => new ParsedFile("Non-existing-file.txt"));
        }

        [Fact]
        public void DirectoryNotFoundException()
        {
            Assert.Throws<DirectoryNotFoundException>(() => new ParsedFile("NonExistingDirectory/Non-existing-file.txt"));
        }

        [Fact(Skip = "No exception is thrown in Linux (CI env), Issue #1")]
        public void IOException()
        {
            const string fileName = "Any.txt";

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                Assert.Throws<IOException>(() => new ParsedFile(fileName));
            }
        }

        [Fact]
        public void ArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new ParsedFile(string.Empty));
            Assert.Throws<ArgumentException>(() => new ParsedFile(""));
        }

        [Fact]
        public void ParsingException()
        {
            IParsedFile file = new ParsedFile(_validPath + "Sample_file.txt");

            IParsedLine line = file.NextLine();

            while (!file.Empty)
            {
                line = file.NextLine();
            }

            Assert.Throws<ParsingException>(() => file.NextLine());

            while (!line.Empty)
            {
                line.NextElement<object>();
            }

            Assert.Throws<ParsingException>(() => line.NextElement<object>());

            Queue<string> testQueue = new Queue<string>(new string[] { string.Empty });
            ParsedLine testLine = new ParsedLine(testQueue);
            Assert.Throws<ParsingException>(() => testLine.NextElement<char>());

            const string stringNull = null;
            testQueue = new Queue<string>(new string[] { stringNull });
            testLine = new ParsedLine(testQueue);

            Assert.Throws<ParsingException>(() => testLine.NextElement<char>());
        }
    }
}
