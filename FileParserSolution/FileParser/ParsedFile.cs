﻿using System.Collections.Generic;

namespace FileParser
{
    public class ParsedFile :IParsedFile
    {
        private Queue<Queue<string>> _value;

        public int Count { get => _value.Count; }

        public bool Empty { get => Count == 0; }

        public ParsedFile(Queue<Queue<string>> parsedFile)
        {
            _value = parsedFile;
        }

        public ParsedFile(string path, char[] existingSeparator = null, string lineSeparatorToAdd = null)
        {
            _value = FileReader.ParseFile(path, existingSeparator, lineSeparatorToAdd);
        }

        // TODO: DECIDE
        // Is it worth keeping a null as return value, or throw exception as in ParsedLine (compulsory behavior in that case)
        public ParsedLine NextLine()
        {
            return _value.Count != 0
                ? new ParsedLine(_value.Dequeue())
                : null;
        }

        // TODO: DECIDE
        // Is it worth keeping a null as return value, or throw exception as in ParsedLine (compulsory behavior in that case)
        public ParsedLine PeekNextLine()
        {
            return _value.Count != 0
                ? new ParsedLine(_value.Peek())
                : null;
        }
    }
}
