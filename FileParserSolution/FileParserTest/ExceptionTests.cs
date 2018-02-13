using System;
using System.Linq;
using System.IO;
using Xunit;

using FileParser;

namespace FileParserTest
{
    public class ExceptionTests
    {
        [Fact]
        void NotImplementedException()
        {
            string fileName = "Any.txt";
            string sampleContent = "Any";

            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                writer.WriteLine(sampleContent);
            }

            Assert.Throws<NotImplementedException>(() => FileReader.Parse<uint>(fileName).ToList());
            Assert.Throws<NotImplementedException>(() => FileReader.Parse<DateTime>(fileName).ToList());
        }

        [Fact]
        void FileNotFoundException()
        {
            Assert.Throws<FileNotFoundException>(() => FileReader.Parse("Non-existing-file.txt"));
        }

        [Fact]
        void DirectoryNotFoundException()
        {
            Assert.Throws<DirectoryNotFoundException>(() => FileReader.Parse("NonExistingDirectory/Non-existing-file.txt"));
        }

        [Fact(Skip ="No exception is thrown in Linux (CI env)")]
        void IOException()
        {
            string fileName = "Any.txt";
            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                Assert.Throws<IOException>(() => FileReader.Parse(fileName));
            }
        }

        [Fact]
        void ArgumentException()
        {
            Assert.Throws<ArgumentException>(() => FileReader.Parse<uint>(string.Empty).ToList());
            Assert.Throws<ArgumentException>(() => FileReader.Parse<DateTime>("").ToList());
        }
    }
}
