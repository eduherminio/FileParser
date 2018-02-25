using System;
using System.Linq;
using System.IO;
using Xunit;

using FileParser;
using System.Collections.Generic;

namespace FileParserTest
{
    public class ExceptionTests
    {
        private readonly string _validPath = "TestFiles" + System.IO.Path.DirectorySeparatorChar;

        [Fact]
        void NotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => FileReader.ParseArray<uint>("Anything.txt"));
            Assert.Throws<NotSupportedException>(() => FileReader.ParseArray<DateTime>("Anything.txt"));
            var aux = new Queue<string>();
            Assert.Throws<NotSupportedException>(() => FileReader.Extract<ulong>(ref aux));


            Queue<Queue<string>> parsedFile = FileReader.ParseFile(_validPath + "Sample_file.txt");
            Queue<string> firstParsedLine = parsedFile.Dequeue();

            Assert.Throws<NotSupportedException>(() => FileReader.Extract<char>(ref firstParsedLine));
        }

        [Fact]
        void FileNotFoundException()
        {
            Assert.Throws<FileNotFoundException>(() => FileReader.ParseFile("Non-existing-file.txt"));
        }

        [Fact]
        void DirectoryNotFoundException()
        {
            Assert.Throws<DirectoryNotFoundException>(() => FileReader.ParseFile("NonExistingDirectory/Non-existing-file.txt"));
        }

        [Fact(Skip ="No exception is thrown in Linux (CI env)")]
        void IOException()
        {
            string fileName = "Any.txt";
            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                Assert.Throws<IOException>(() => FileReader.ParseFile(fileName));
            }
        }

        [Fact]
        void ArgumentException()
        {
            Assert.Throws<ArgumentException>(() => FileReader.ParseFile(string.Empty).ToList());
            Assert.Throws<ArgumentException>(() => FileReader.ParseFile("").ToList());
        }

        [Fact]
        void ParsingException()
        {
            IParsedFile file = new ParsedFile(_validPath + "Sample_file.txt");

            IParsedLine line = file.NextLine();

            while (!file.Empty)
                line = file.NextLine();

            Assert.Throws<ParsingException>(() => file.NextLine());

            while (!line.Empty)
                line.NextElement<object>();

            Assert.Throws<ParsingException>(() => line.NextElement<object>());

            string stringEmpty = string.Empty;
            Assert.Throws<ParsingException>(() => FileReader.ExtractChar(ref stringEmpty));
            string stringNull = null;
            Assert.Throws<ParsingException>(() => FileReader.ExtractChar(ref stringNull));
        }
    }
}
