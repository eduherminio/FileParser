using System;
using System.Collections.Generic;
using System.ComponentModel;

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
        /// Optional parameter: an already known typeConverter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        static public T Convert<T>(string str, TypeConverter typeConverter = null)
        {
            if (typeof(T).IsPrimitive)
            {
                return typeConverter == null
                    ? TConverter.ChangeType<T>(str)
                    : TConverter.ChangeType<T>(str, typeConverter);
            }
            else // Avoids exception if T is an object
            {
                object o = str;
                return (T)o;
            }
        }

        static public TypeConverter GetConverter<T>()
        {
            if (typeof(T).IsPrimitive)
            {
                return TConverter.GetTypeConverter(typeof(T));
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
