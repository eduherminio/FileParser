using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace FileParser
{
    /// <summary>
    /// Provides generic type conversions for basic types, including nullable ones
    /// Original method by Tuna Toksoz
    /// Sources:
    /// https://stackoverflow.com/questions/8625/generic-type-conversion-from-string
    /// http://web.archive.org/web/20101214042641/http://dogaoztuzun.com/post/C-Generic-Type-Conversion.aspx
    /// </summary>
    static internal class TConverter
    {
        static internal T ChangeType<T>(object value, TypeConverter typeConverter = null)
        {
            return typeConverter == null
                    ? (T)ChangeType(typeof(T), value)
                    : (T)typeConverter.ConvertFrom(value);
        }

        static private object ChangeType(Type t, object value)
        {
            if (t == typeof(double))
            {
                return ParseDouble(value);
            }
            else
            {
                TypeConverter tc = TypeDescriptor.GetConverter(t);

                return tc.ConvertFrom(value);
            }
        }

        private static object ParseDouble(object value)
        {
            double result;

            string doubleAsString = value.ToString();
            IEnumerable<char> doubleAsCharList = doubleAsString.ToList();

            if (doubleAsCharList.Where(ch => ch == '.' || ch == ',').Count() <= 1)
            {
                double.TryParse(doubleAsString.Replace(',', '.'),
                    System.Globalization.NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out result);
            }
            else
            {
                if (doubleAsCharList.Where(ch => ch == '.').Count() <= 1
                    && doubleAsCharList.Where(ch => ch == ',').Count() > 1)
                {
                    double.TryParse(doubleAsString.Replace(",", string.Empty),
                        System.Globalization.NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out result);
                }
                else if (doubleAsCharList.Where(ch => ch == ',').Count() <= 1
                    && doubleAsCharList.Where(ch => ch == '.').Count() > 1)
                {
                    double.TryParse(doubleAsString.Replace(".", string.Empty).Replace(',', '.'),
                        System.Globalization.NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out result);
                }
                else
                {
                    throw new ParsingException($"Error parsing {doubleAsString} as double, try removing thousand separators (if any)");
                }
            }

            return result as object;
        }

        static internal TypeConverter GetTypeConverter(Type t)
        {
            return TypeDescriptor.GetConverter(t);
        }

        /// <summary>
        /// Currently unused
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TC"></typeparam>
        static internal void RegisterTypeConverter<T, TC>() where TC : TypeConverter
        {
            TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TC)));
        }
    }
}
