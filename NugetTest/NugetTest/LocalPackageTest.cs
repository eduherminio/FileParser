using System;
using System.Collections.Generic;
using Xunit;

using FileParser;

namespace NugetTest
{
    public class LocalPackageTest
    {
        [Fact]
        public void Test()
        {
            List<int> numberList = new List<int>();
            List<string> stringList = new List<string>();

            IParsedFile file = new ParsedFile("Sample_file.txt");

            IParsedLine firstLine = file.NextLine();

            int n = firstLine.NextElement<int>();
            string str = firstLine.NextElement<string>();

            while (!file.Empty)
            {
                IParsedLine line = file.NextLine();
                int counter = line.NextElement<int>();
                for (int j = 0; j < counter; ++j)
                    numberList.Add(line.NextElement<int>());

                while (!line.Empty)
                    stringList.Add(line.NextElement<string>());

                System.Diagnostics.Debug.WriteLine(String.Join(", ", numberList));
                System.Diagnostics.Debug.WriteLine(str + ": " + String.Join(", ", stringList));
            }
        }
    }
}
