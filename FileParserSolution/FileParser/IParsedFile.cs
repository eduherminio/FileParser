namespace FileParser
{
    public interface IParsedFile
    {
        /// <summary>
        /// Returns the size (number of lines) of ParsedFile
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Returns whether ParsedFile has no elements
        /// </summary>
        bool Empty { get; }

        /// <summary>
        /// Returns next line (IParsedLine), removing it from ParsedFile
        /// </summary>
        /// <exception>
        /// ParsingException if file is already empty
        /// </exception>
        /// <returns></returns>
        IParsedLine NextLine();

        /// <summary>
        /// Returns next line (IParsedLine), not modifying ParsedFile
        /// </summary>
        /// <exception>
        /// ParsingException if file is already empty
        /// </exception>
        /// <returns></returns>
        IParsedLine PeekNextLine();

    }
}
