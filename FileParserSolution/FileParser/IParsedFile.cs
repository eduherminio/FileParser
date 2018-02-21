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
        /// Returns next line in form of a ParsedLine instance, removing it from ParsedFile
        /// </summary>
        /// <returns></returns>
        ParsedLine NextLine();

        /// <summary>
        /// Returns next line in form of a ParsedLine instance, not modifying ParsedFile
        /// </summary>
        /// <returns></returns>
        ParsedLine PeekNextLine();

    }
}
