using System;
using System.Collections.Generic;

using FileParser;

namespace ConsoleParserApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press a key to start");
            Console.ReadKey();

            ICollection<string> sampleSpaces = FileReader.Parse("Sample_spaces.txt");

            Console.WriteLine("Press a key to end");
            Console.ReadKey();
        }
    }
}
