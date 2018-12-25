using System.Collections.Generic;

namespace FileParser
{
    public interface IParsedLine
    {
        /// <summary>
        /// Returns the size (number of elements) of ParsedLine
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Returns whether ParsedLine has no elements
        /// </summary>
        bool Empty { get; }

        /// <summary>
        /// Returns next element of type T, removing it from ParsedLine
        /// </summary>
        /// <exception>
        /// ParsingException if line is already empty
        /// </exception>
        /// <returns></returns>
        T NextElement<T>();

        /// <summary>
        /// Returns next element of type T, not modifying ParsedLine
        /// This still allows its modification
        /// </summary>
        /// <exception>
        /// ParsingException if line is already empty
        /// </exception>
        /// <returns></returns>
        T PeekNextElement<T>();

        /// <summary>
        /// Returns remaining elements as a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> ToList<T>();

        /// <summary>
        /// Returns remaining elements as a single string, separated by wordSeparator
        /// </summary>
        /// <param name="wordSeparator"></param>
        /// <returns></returns>
        string ToSingleString(string wordSeparator = " ");
    }
}
