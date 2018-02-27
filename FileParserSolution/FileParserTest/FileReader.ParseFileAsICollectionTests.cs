using System.Linq;
using System.Collections.Generic;
using Xunit;

using FileParser;

namespace FileParserTest
{
    public class ParseFileAsICollectionTests
    {
        private string _sampleFolderPath = "TestFiles" + System.IO.Path.DirectorySeparatorChar;

        [Fact]
        public void SameFileDifferentSeparators()
        {
            ICollection<string> sampleSpaces = new ParsedFile(_sampleFolderPath + "Sample_spaces.txt").ToList<string>();
            ICollection<string> sampleCommas = new ParsedFile(_sampleFolderPath + "Sample_commas.txt", new char[] { ',' }).ToList<string>();
            ICollection<string> sampleSlashes = new ParsedFile(_sampleFolderPath + "Sample_doubleslashes.txt", new char[] { '/', '/' }).ToList<string>();

            Assert.True(sampleSpaces.Count > 1);

            Assert.True(Enumerable.SequenceEqual(sampleSpaces, sampleCommas));
            Assert.True(Enumerable.SequenceEqual(sampleSpaces, sampleSlashes));
        }

        [Fact]
        public void DifferentFilesSameSeparators()
        {
            ICollection<string> sampleSlashes = new ParsedFile(_sampleFolderPath + "Sample_doubleslashes.txt", new char[] { '/', '/' }).ToList<string>();
            ICollection<string> modififedSampleSlashes = new ParsedFile(_sampleFolderPath + "SlightlyModified_Sample_doubleslashes.txt", new char[] { '/', '/' }).ToList<string>();

            Assert.True(sampleSlashes.Count > 1);

            Assert.NotEqual(sampleSlashes, modififedSampleSlashes);
        }
    }
}
