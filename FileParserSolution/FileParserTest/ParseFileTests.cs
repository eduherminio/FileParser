using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

using FileParser;

namespace FileParserTest
{
    public class ParseFileTests
    {
        [Fact]
        void BasicParseFileTest()
        {
            // Sample file:
            // First line has a random number and a category of aliments.
            // Following lines firstly indicates how many numeric elements are following.
            // After those numeric elements, they includes an unknown number of items.

            string fileName = "CustomFileParse.txt";
            string line0 = " 23  food    ";
            string line1 = "  3  100 200 300    apple peer ";
            string line2 = " 4  400   500    600 700 banana    meat  fish       ";

            int expectedInitialNumber = 23;
            string expectedCategory = "food";
            List<string> expectedFood = new List<string>() { "apple", "peer", "banana", "meat", "fish" };
            List<List<int>> expectedNumbers = new List<List<int>>()
            {
                new List<int>() { 100, 200, 300},
                new List<int>() { 400, 500, 600, 700 }
            };

            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                writer.WriteLine(line0);
                writer.WriteLine(line1);
                writer.WriteLine(line2);
            }

            int initialNumber = -1;
            string category = null;
            List<string> food = new List<string>();
            List<List<int>> numbers = new List<List<int>>();

            {
                Queue<Queue<string>> parsedFile = FileReader.ParseFile(fileName);

                Queue<string> firstLine = parsedFile.Dequeue();
                initialNumber = FileReader.Extract<int>(firstLine);
                category = FileReader.Extract<string>(firstLine);

                while (parsedFile.Count > 0)
                {
                    Queue<string> line = parsedFile.Dequeue();

                    List<int> listNumbers = new List<int>(FileReader.Extract<int>(line));
                    for (int i = 0; i < listNumbers.Capacity; ++i)
                        listNumbers.Add(FileReader.Extract<int>(line));

                    numbers.Add(listNumbers);

                    while (line.Count > 0)
                        food.Add(FileReader.Extract<string>(line));
                }
            }

            Assert.Equal(expectedInitialNumber, initialNumber);
            Assert.Equal(expectedCategory, category);
            Assert.Equal(expectedFood, food);
            for (int i = 0; i < numbers.Count; ++i)
            {
                Assert.Equal(expectedNumbers.ElementAt(i), numbers.ElementAt(i));
            }
        }
    }
}
