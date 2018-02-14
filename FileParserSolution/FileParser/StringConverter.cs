using System;
using System.Collections.Generic;

namespace FileParser
{
    static public class StringConverter
    {
        /// <summary>
        /// Tested conversions
        /// </summary>
        static public HashSet<Type> SupportedTypes
        {
            get => new HashSet<Type>()
            {
                typeof(bool),
                typeof(string),
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(double)
            };
        }

        /// <summary>
        /// Converts strings to basic, nullable types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        static public T Convert<T>(string str)
        {
            if (typeof(T).IsPrimitive)
            {
                return TConverter.ChangeType<T>(str);
            }
            else // Avoids exception if T is an object
            {
                object o = str;
                return (T)o;
            }
        }
    }
}
