using Xunit;

namespace FileParser.Test;

public class ExceptionTests
{
    private readonly string _validPath = "TestFiles" + Path.DirectorySeparatorChar;

    [Fact]
    public void NotSupportedException()
    {
        Assert.Throws<NotSupportedException>(() => new ParsedFile(_validPath + "Sample_file.txt").ToList<DateTime>());
        Assert.Throws<NotSupportedException>(() => ParsedFile.ReadAllGroupsOfLines<DateTime>(_validPath + "Sample_file.txt"));

        IParsedFile parsedFile = new ParsedFile(_validPath + "Sample_file.txt");
        IParsedLine firstParsedLine = parsedFile.NextLine();

        Assert.Throws<NotSupportedException>(() => firstParsedLine.NextElement<char>());
        Assert.Throws<NotSupportedException>(() => ParsedFile.ReadAllGroupsOfLines<char>(_validPath + "Sample_file.txt"));
    }

    [Fact]
    public void FileNotFoundException()
    {
        Assert.Throws<FileNotFoundException>(() => new ParsedFile("Non-existing-file.txt"));
    }

    [Fact]
    public void DirectoryNotFoundException()
    {
        Assert.Throws<DirectoryNotFoundException>(() => new ParsedFile("NonExistingDirectory/Non-existing-file.txt"));
    }

    [Fact(Skip = "No exception is thrown in Linux (CI env), Issue #1")]
    public void IOException()
    {
        const string fileName = "Any.txt";

        using StreamWriter writer = new(fileName);
        Assert.Throws<IOException>(() => new ParsedFile(fileName));
    }

    [Fact]
    public void ArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new ParsedFile(string.Empty));
        Assert.Throws<ArgumentException>(() => new ParsedFile(""));
    }

    [Fact]
    public void ParsingException()
    {
        IParsedFile file = new ParsedFile(_validPath + "Sample_file.txt");

        IParsedLine line = file.NextLine();

        while (!file.Empty)
        {
            line = file.NextLine();
        }

        Assert.Throws<ParsingException>(() => file.NextLine());

        while (!line.Empty)
        {
            line.NextElement<object>();
        }

        Assert.Throws<ParsingException>(() => line.NextElement<object>());

        Queue<string> testQueue = new(new string[] { string.Empty });
        ParsedLine testLine = new(testQueue);
        Assert.Throws<ParsingException>(() => testLine.NextElement<char>());
    }
}
