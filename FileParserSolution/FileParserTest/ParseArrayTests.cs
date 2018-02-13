using System.Collections.Generic;
using System.Linq;
using System.IO;
using Xunit;

using FileParser;

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

            List<string> parsedArray = FileReader.Parse<string>(fileName, separator).ToList();
            Assert.Equal(parsedArray, new List<string>() { "one", "two", "three", "four"});

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

            List<bool> parsedArray = FileReader.Parse<bool>(fileName).ToList();
            Assert.Equal(parsedArray, new List<bool>() { true, false, true });
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

            List<short> parsedArray = FileReader.Parse<short>(fileName).ToList();
            Assert.Equal(parsedArray, new List<short>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
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

            List<int> parsedArray = FileReader.Parse<int>(fileName).ToList();
            Assert.Equal(parsedArray, new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
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

            List<long> parsedArray = FileReader.Parse<long>(fileName).ToList();
            Assert.Equal(parsedArray, new List<long>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        }

        [Fact]
        public void ArrayOfDoubles()
        {
            string fileName = "Sample_arrayofdoubles.txt";
            string sampleContent = " 0,0 1,1 2,2  3,3  4    5,5 6,6 7,7 8,8 9 ";

            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                writer.WriteLine(sampleContent);
            }

            List<double> parsedArray = FileReader.Parse<double>(fileName).ToList();
            Assert.Equal(parsedArray, new List<double>() { 0, 1.1, 2.2, 3.3, 4.0, 5.5, 6.6, 7.7, 8.8, 9.00000 });
        }
    }
}
