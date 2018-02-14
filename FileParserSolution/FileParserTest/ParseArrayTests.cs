using System.Collections.Generic;
using System.Linq;
using System.IO;
using Xunit;

using FileParser;
using System;

namespace FileParserTest
{
    public class ParseArrayTests
    {
        [Fact]
        public void ArrayOfStrings()
        {
            string fileName = "Sample_arrayofints.txt";
            char[] separator = "$$$$$$$".ToCharArray();
            string sampleContent = "   $$$$$$$one$$$$$$$two$$$$$$$three$$$$$$$four$$$$$$$   ";

            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                writer.WriteLine(sampleContent);
            }

            List<string> parsedArray = FileReader.ParseArray<string>(fileName, separator).ToList();
            Assert.Equal(parsedArray, new List<string>() { "one", "two", "three", "four" });

        }

        [Fact]
        public void ArrayOfBools()
        {
            string fileName = "Sample_arrayofbools.txt";
            string sampleContent = " true false true";

            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                writer.WriteLine(sampleContent);
            }

            List<bool> parsedArray = FileReader.ParseArray<bool>(fileName).ToList();
            Assert.Equal(new List<bool>() { true, false, true }, parsedArray);
        }

        [Fact]
        public void ArrayOfShorts()
        {
            string fileName = "Sample_arrayofints.txt";
            string sampleContent = " 0 1 2  3  4    5 6 7 8 9 ";

            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                writer.WriteLine(sampleContent);
            }

            List<short> parsedArray = FileReader.ParseArray<short>(fileName).ToList();
            Assert.Equal(new List<short>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, parsedArray);
        }

        [Fact]
        public void ArrayOfInts()
        {
            string fileName = "Sample_arrayofints.txt";
            string sampleContent = " 0 1 2  3  4    5 6 7 8 9 ";

            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                writer.WriteLine(sampleContent);
            }

            List<int> parsedArray = FileReader.ParseArray<int>(fileName).ToList();
            Assert.Equal(new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, parsedArray);
        }

        [Fact]
        public void ArrayOfLongs()
        {
            string fileName = "Sample_arrayofints.txt";
            string sampleContent = " 0 1 2  3  4    5 6 7 8 9 ";

            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                writer.WriteLine(sampleContent);
            }

            List<long> parsedArray = FileReader.ParseArray<long>(fileName).ToList();
            Assert.Equal(new List<long>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, parsedArray);
        }

        [Fact]
        public void ArrayOfDoubles()
        {
            string fileName = "Sample_arrayofdoubles.txt";
            ICollection<double> vectorOfDoubles = new List<double>() { 0.0, 1.10, 2.2, 3.30, 4, 5.5, 6.60, 7.7, 8.8000, 9 };
            string vectorToWrite = String.Join(' ', vectorOfDoubles);   // Avoiding dependency on culture (. or ,)

            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                writer.WriteLine(vectorToWrite);
            }

            List<double> parsedArray = FileReader.ParseArray<double>(fileName).ToList();
            Assert.Equal(new List<double>() { 0, 1.1, 2.2, 3.3, 4.0, 5.5, 6.6, 7.7, 8.8, 9.00000 }, parsedArray);
        }
    }
}
