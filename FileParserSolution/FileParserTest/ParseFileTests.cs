using System.Linq;
using System.Collections.Generic;
using Xunit;

using FileParser;

namespace FileParserTest
{
    public class ParseFileTests
    {
        private string _sampleFolderPath = "TestFiles" + System.IO.Path.DirectorySeparatorChar;

        [Fact]
        public void SameFileDifferentSeparators()
        {
            ICollection<string> sampleSpaces = FileReader.Parse(_sampleFolderPath + "Sample_spaces.txt");
            ICollection<string> sampleCommas = FileReader.Parse(_sampleFolderPath + "Sample_commas.txt", new char[] { ',' });
            ICollection<string> sampleSlashes = FileReader.Parse(_sampleFolderPath + "Sample_doubleslashes.txt", new char[] { '/', '/' });

            Assert.True(sampleSpaces.Count > 1);

            Assert.True(Enumerable.SequenceEqual(sampleSpaces, sampleCommas));
            Assert.True(Enumerable.SequenceEqual(sampleSpaces, sampleSlashes));
        }

        [Fact]
        public void DifferentFilesSameSeparators()
        {
            ICollection<string> sampleSlashes = FileReader.Parse(_sampleFolderPath + "Sample_doubleslashes.txt", new char[] { '/', '/' });
            ICollection<string> modiifedSampleSlashes = FileReader.Parse(_sampleFolderPath + "SlightlyModified_Sample_doubleslashes.txt", new char[] { '/', '/' });

            Assert.True(sampleSlashes.Count > 1);
            Assert.Equal(sampleSlashes.Count, modiifedSampleSlashes.Count);

            Assert.False(Enumerable.SequenceEqual(sampleSlashes, modiifedSampleSlashes));
        }
    }
}
