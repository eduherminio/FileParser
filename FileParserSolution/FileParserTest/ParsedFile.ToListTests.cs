using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Xunit;

using FileParser;

namespace FileParserTest
{
    public class ToListTests
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

        [Fact]
        public void ListOfStrings()
        {
            string fileName = "Sample_ListOfints.txt";
            char[] separator = "$$$$$$$".ToCharArray();
            string sampleContent = "   $$$$$$$one$$$$$$$two$$$$$$$three$$$$$$$four$$$$$$$   ";

            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                writer.WriteLine(sampleContent);
            }

            List<string> parsedArray = new ParsedFile(fileName, separator).ToList<string>();
            Assert.Equal(new List<string>() { "one", "two", "three", "four" }, parsedArray);
        }

        [Fact]
        public void ListOfBools()
        {
            string fileName = "Sample_ListOfbools.txt";
            string sampleContent = " true false true";

            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                writer.WriteLine(sampleContent);
            }

            List<bool> parsedArray = new ParsedFile(fileName).ToList<bool>();
            Assert.Equal(new List<bool>() { true, false, true }, parsedArray);
        }

        [Fact]
        public void ListOfShorts()
        {
            string fileName = "Sample_ListOfints.txt";
            string sampleContent = " 0 1 2  3  4    5 6 7 8 9 ";

            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                writer.WriteLine(sampleContent);
            }

            List<short> parsedArray = new ParsedFile(fileName).ToList<short>();
            Assert.Equal(new List<short>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, parsedArray);
        }

        [Fact]
        public void ListOfInts()
        {
            string fileName = "Sample_ListOfints.txt";
            string sampleContent = " 0 1 2  3  4    5 6 7 8 9 ";

            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                writer.WriteLine(sampleContent);
            }

            List<int> parsedArray = new ParsedFile(fileName).ToList<int>();
            Assert.Equal(new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, parsedArray);
        }

        [Fact]
        public void ListOfLongs()
        {
            string fileName = "Sample_ListOfints.txt";
            string sampleContent = " 0 1 2  3  4    5 6 7 8 9 ";

            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                writer.WriteLine(sampleContent);
            }

            List<long> parsedArray = new ParsedFile(fileName).ToList<long>();
            Assert.Equal(new List<long>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, parsedArray);
        }

        [Fact]
        public void ListOfDoubles()
        {
            string fileName = "Sample_ListOfdoubles.txt";
            ICollection<double> vectorOfDoubles = new List<double>() { 0.0, 1.10, 2.2, 3.30, 4, 5.5, 6.60, 7.7, 8.8000, 9 };
            string vectorToWrite = String.Join(' ', vectorOfDoubles);   // Avoiding dependency on culture (. or ,)

            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                writer.WriteLine(vectorToWrite);
            }

            List<double> parsedArray = new ParsedFile(fileName).ToList<double>();
            Assert.Equal(new List<double>() { 0, 1.1, 2.2, 3.3, 4.0, 5.5, 6.6, 7.7, 8.8, 9.00000 }, parsedArray);
        }
    }
}
