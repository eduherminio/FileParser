using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

using FileParser;

namespace FileParserTest
{
    public class ExtractTests
    {
        [Fact]
        void TypicalUC()
        {
            string fileName = "TypicalUC.txt";
            string sampleContent = "  3  1154 508 100    vegetable ";

            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                writer.WriteLine(sampleContent);
            }

            List<long> expectedList = new List<long>() { 1154, 508, 100 };
            string expectedString = "vegetable";

            Queue<string> queue = new Queue<string>(FileReader.ParseLine(fileName));

            int n_ints = FileReader.Extract<int>(queue);

            for (int i = 0; i < n_ints; ++i)
            {
                long data = FileReader.Extract<long>(queue);
                Assert.Equal(expectedList.ElementAt(i), data);
            }

            string rawString = queue.Peek();    // Raw string
            string lastString = FileReader.Extract<string>(queue); // Converted string => string
            Assert.Equal(rawString, lastString);

            Assert.Equal(expectedString, lastString);

            Assert.Empty(queue);
        }
    }
}
