using System.Text;
using Xunit;

namespace FileParser.Test.ParsedFileTest;

public class ExtractTests
{
    [Fact]
    public void CustomLineParse()
    {
        const string fileName = "CustomLineParse.txt";
        const string sampleContent = "  3  1154 508 100    vegetable ";

        List<long> expectedList = [1154, 508, 100];
        const string expectedString = "vegetable";

        using (StreamWriter writer = new(fileName))
        {
            writer.WriteLine(sampleContent);
        }

        IParsedLine line = new ParsedFile(fileName).NextLine();

        int n_ints = line.NextElement<int>();

        for (int i = 0; i < n_ints; ++i)
        {
            long data = line.NextElement<long>();
            Assert.Equal(expectedList[i], data);
        }

        string rawString = line.PeekNextElement<string>();    // Raw string
        string lastString = line.NextElement<string>(); // Converted string => string
        Assert.Equal(rawString, lastString);

        Assert.Equal(expectedString, lastString);

        Assert.True(line.Empty);
    }

    [Fact]
    public void ExtractChar()
    {
        const string fileName = "ExtractChar.txt";
        const string line1 = "+-*/!?#$%&";
        const string line2 = "@()[]{}\"";

        StringBuilder parsedFile = new(string.Empty);

        using (StreamWriter writer = new(fileName))
        {
            writer.WriteLine(Math.PI.ToString());
            writer.WriteLine(line1);
            writer.WriteLine(line2);
        }

        var file = new ParsedFile(fileName);
        var firstParsedLine = file.NextLine();
        int nLines = Convert.ToInt32(Math.Floor(firstParsedLine.NextElement<double>())) - 1;
        for (int iLine = 0; iLine < nLines; ++iLine)
        {
            IParsedLine parsedLine = file.NextLine();
            while (!parsedLine.Empty)
            {
                parsedFile.Append(parsedLine.NextElement<char>());
            }

            parsedFile.Append('\\');
            Assert.True(parsedLine.Empty);
        }
        Assert.True(file.Empty);

        Assert.Equal(line1 + '\\' + line2 + '\\', parsedFile.ToString());
    }

    [Fact]
    public void MultipleCharWordSeparator()
    {
        const string fileName = "MultipleCharWordSeparator.txt";
        const string line1 = "   FirstName|| LastName  ||Age||EyeColor ";
        const string line2 = "           John|| Doe||66 ||  Brown";

        var expectedFirstLine = new[] { "FirstName", "LastName", "Age", "EyeColor" };
        var expectedSecondLine = new[] { "John", "Doe", "66", "Brown" };

        using (StreamWriter writer = new(fileName))
        {
            writer.WriteLine(line1);
            writer.WriteLine(line2);
        }

        var file = new ParsedFile(fileName, "||");

        int index = 0;
        var line = file.NextLine();
        while (!line.Empty)
        {
            var item = line.NextElement<string>();
            Assert.Equal(expectedFirstLine[index++], item);
        }

        line = file.NextLine();
        Assert.Equal(expectedSecondLine, line.ToList<string>());

        Assert.True(file.Empty);
    }

    [Fact]
    public void ParseCsvFileIntoStrings()
    {
        const string fileName = "ParseCsvFileIntoStrings.txt";
        const string line1 = "FirstName,LastName,Age,EyeColor";
        const string line2 = "John,Doe,66,Brown";
        const string line3 = "Cthulhu,,1000,Black";
        const string line4 = "Bugs,Bunny,33,White";

        using (StreamWriter writer = new(fileName))
        {
            writer.WriteLine(line1);
            writer.WriteLine(line2);
            writer.WriteLine(line3);
            writer.WriteLine(line4);
        }

        var file = new ParsedFile(fileName, ",", ignoreEmptyItems: false);

        var headerLine = file.NextLine();
        var buckets = new List<List<string>>(headerLine.ToList<string>().Select(header => new List<string>(1000) { ModifyString(header) }));

        while (!file.Empty)
        {
            var line = file.NextLine().ToList<string>();
            for (int index = 0; index < line.Count; ++index)
            {
                buckets[index].Add(ModifyString(line[index]));
            }
        }

        static string ModifyString(string str) => str.ToLowerInvariant();

        string[] expected = ["firstname", "john", "cthulhu", "bugs"];
        Assert.Equal(expected, buckets[0]);

        string[] expected1 = ["lastname", "doe", "", "bunny"];
        Assert.Equal(expected1, buckets[1]);

        string[] expected2 = ["age", "66", "1000", "33"];
        Assert.Equal(expected2, buckets[2]);

        string[] expected3 = ["eyecolor", "brown", "black", "white"];
        Assert.Equal(expected3, buckets[3]);
    }

    [Fact]
    public void ParseCsvFile()
    {
        const string fileName = "ParseCsvFile.txt";
        const string line1 = "Name,Age,Score";
        const string line2 = "Abraham,10,9.05";
        const string line3 = "Babilon,,64.05";
        const string line4 = "Croc,99,";

        using (StreamWriter writer = new(fileName))
        {
            writer.WriteLine(line1);
            writer.WriteLine(line2);
            writer.WriteLine(line3);
            writer.WriteLine(line4);
        }

        var file = new ParsedFile(fileName, ",", ignoreEmptyItems: false);

        file.NextLine();

        var line = file.NextLine();

        Assert.Equal("Abraham", line.NextElement<string>());
        Assert.Equal(10, line.NextElement<int>());
        Assert.Equal(9.05, line.NextElement<double>());

        line = file.NextLine();

        Assert.Equal("Babilon", line.NextElement<string>());
        Assert.Equal(default, line.NextElement<int>());
        Assert.Equal(64.05, line.NextElement<double>());

        line = file.NextLine();

        Assert.Equal("Croc", line.NextElement<string>());
        Assert.Equal(99, line.NextElement<int>());
        Assert.Equal(default, line.NextElement<double>());

        Assert.True(file.Empty);
    }
}
