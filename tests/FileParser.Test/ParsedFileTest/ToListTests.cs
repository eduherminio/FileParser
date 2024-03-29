using Xunit;

namespace FileParser.Test.ParsedFileTest
{
    public class ToListTests
    {
        private readonly string _sampleFolderPath = "TestFiles" + Path.DirectorySeparatorChar;

        [Fact]
        public void SameFileDifferentSeparators()
        {
            ICollection<string> sampleSpaces = new ParsedFile(_sampleFolderPath + "Sample_spaces.txt").ToList<string>();
            ICollection<string> sampleCommas = new ParsedFile(_sampleFolderPath + "Sample_commas.txt", new char[] { ',' }).ToList<string>();
            ICollection<string> sampleCommas2 = new ParsedFile(_sampleFolderPath + "Sample_commas.txt", ",").ToList<string>();
            ICollection<string> sampleSlashes = new ParsedFile(_sampleFolderPath + "Sample_doubleslashes.txt", new char[] { '/', '/' }).ToList<string>();
            ICollection<string> sampleSlashes2 = new ParsedFile(_sampleFolderPath + "Sample_doubleslashes.txt", "//").ToList<string>();

            Assert.True(sampleSpaces.Count > 1);

            Assert.True(sampleSpaces.SequenceEqual(sampleCommas));
            Assert.True(sampleSpaces.SequenceEqual(sampleCommas2));
            Assert.True(sampleSpaces.SequenceEqual(sampleSlashes));
            Assert.True(sampleSpaces.SequenceEqual(sampleSlashes2));
        }

        [Fact]
        public void DifferentFilesSameSeparators()
        {
            ICollection<string> sampleSlashes = new ParsedFile(_sampleFolderPath + "Sample_doubleslashes.txt", new char[] { '/', '/' }).ToList<string>();
            ICollection<string> modififedSampleSlashes = new ParsedFile(_sampleFolderPath + "SlightlyModified_Sample_doubleslashes.txt", new char[] { '/', '/' }).ToList<string>();
            ICollection<string> modififedSampleSlashes2 = new ParsedFile(_sampleFolderPath + "SlightlyModified_Sample_doubleslashes.txt", "//").ToList<string>();

            Assert.True(sampleSlashes.Count > 1);

            Assert.NotEqual(sampleSlashes, modififedSampleSlashes);
            Assert.NotEqual(sampleSlashes, modififedSampleSlashes2);
        }

        [Fact]
        public void ListOfStrings()
        {
            const string fileName = "Sample_ListOfints.txt";
            const string separator = "$$$$$$$";
            const string sampleContent = "   $$$$$$$one$$$$$$$two$$$$$$$three$$$$$$$four$$$$$$$   ";

            using (StreamWriter writer = new(fileName))
            {
                writer.WriteLine(sampleContent);
            }

            List<string> parsedArray = new ParsedFile(fileName, separator.ToCharArray()).ToList<string>();
            List<string> parsedArray2 = new ParsedFile(fileName, separator).ToList<string>();
            Assert.Equal(new List<string>() { "one", "two", "three", "four" }, parsedArray);
            Assert.Equal(new List<string>() { "one", "two", "three", "four" }, parsedArray2);
        }

        [Fact]
        public void ListOfBools()
        {
            const string fileName = "Sample_ListOfbools.txt";
            const string sampleContent = " true false true";

            using (StreamWriter writer = new(fileName))
            {
                writer.WriteLine(sampleContent);
            }

            List<bool> parsedArray = new ParsedFile(fileName).ToList<bool>();
            Assert.Equal(new List<bool>() { true, false, true }, parsedArray);
        }

        [Fact]
        public void ListOfShorts()
        {
            const string fileName = "Sample_ListOfints.txt";
            const string sampleContent = " 0 1 2  3  4    5 6 7 8 9 ";

            using (StreamWriter writer = new(fileName))
            {
                writer.WriteLine(sampleContent);
            }

            List<short> parsedArray = new ParsedFile(fileName).ToList<short>();
            Assert.Equal(new List<short>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, parsedArray);
        }

        [Fact]
        public void ListOfInts()
        {
            const string fileName = "Sample_ListOfints.txt";
            const string sampleContent = " 0 1 2  3  4    5 6 7 8 9 ";

            using (StreamWriter writer = new(fileName))
            {
                writer.WriteLine(sampleContent);
            }

            List<int> parsedArray = new ParsedFile(fileName).ToList<int>();
            Assert.Equal(new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, parsedArray);
        }

        [Fact]
        public void ListOfLongs()
        {
            const string fileName = "Sample_ListOfints.txt";
            const string sampleContent = " 0 1 2  3  4    5 6 7 8 9 ";

            using (StreamWriter writer = new(fileName))
            {
                writer.WriteLine(sampleContent);
            }

            List<long> parsedArray = new ParsedFile(fileName).ToList<long>();
            Assert.Equal(new List<long>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, parsedArray);
        }

        [Fact]
        public void ListOfDoubles()
        {
            const string fileName = "Sample_ListOfdoubles.txt";
            ICollection<double> vectorOfDoubles = new List<double>() { 0.0, 1.10, 2.2, 3.30, 4, 5.5, 6.60, 7.7, 8.8000, 9 };
            string vectorToWrite = string.Join(" ", vectorOfDoubles);   // Avoiding dependency on culture (. or ,)

            using (StreamWriter writer = new(fileName))
            {
                writer.WriteLine(vectorToWrite);
            }

            List<double> parsedArray = new ParsedFile(fileName).ToList<double>();
            Assert.Equal(new List<double>() { 0, 1.1, 2.2, 3.3, 4.0, 5.5, 6.6, 7.7, 8.8, 9.00000 }, parsedArray);
        }
    }
}
