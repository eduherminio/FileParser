using System.Collections.Generic;

namespace FileParser
{
    public class ParsedFile :IParsedFile
    {
        Queue<Queue<string>> Value { get; }

        public int Count { get => Value.Count; }

        public bool Empty { get => Count == 0; }

        public ParsedFile(Queue<Queue<string>> parsedFile)
        {
            Value = parsedFile;
        }

        public ParsedFile(string path, char[] existingSeparator = null, string lineSeparatorToAdd = null)
        {
            Value = FileReader.ParseFile(path, existingSeparator, lineSeparatorToAdd);
        }

        // TODO: DECIDE
        // Is it worth keeping a null as return value, or throw exception as in ParsedLine (compulsory behavior in that case)
        public ParsedLine NextLine()
        {
            return Value.Count != 0
                ? new ParsedLine(Value.Dequeue())
                : null;
        }

        // TODO: DECIDE
        // Is it worth keeping a null as return value, or throw exception as in ParsedLine (compulsory behavior in that case)
        public ParsedLine PeekNextLine()
        {
            return Value.Count != 0
                ? new ParsedLine(Value.Peek())
                : null;
        }
    }
}
