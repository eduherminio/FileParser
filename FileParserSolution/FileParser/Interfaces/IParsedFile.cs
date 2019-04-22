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
        /// Returns the line at a specified index
        /// </summary>
        /// <param name="index">zero-based index</param>
        /// <returns>The line at the specified position in file</returns>
        /// <exception cref="System.ArgumentNullException">file is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0 or greater than or equal to the number of lines in file</exception>
        IParsedLine LineAt(int index);

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
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> ToList<T>(string lineSeparatorToAdd = null);

        /// <summary>
        /// Returns remaining elements as a single string, separated by given wordSeparator and lineSeparator
        /// </summary>
        /// <param name="wordSeparator"></param>
        /// <param name="lineSeparator"></param>
        /// <returns></returns>
        string ToSingleString(string wordSeparator = " ", string lineSeparator = null);

        /// <summary>
        /// Appends a line to the end of the file
        /// </summary>
        /// <param name="str"></param>
        void Append(IParsedLine parsedLine);
    }
}
