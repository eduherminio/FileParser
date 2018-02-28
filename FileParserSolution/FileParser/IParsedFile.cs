using System.Collections.Generic;

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

        /// <summary>
        /// Returns remaining elements as a list
        /// TODO test, with T != string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> ToList<T>(string lineSeparatorToAdd = null);
    }
}
