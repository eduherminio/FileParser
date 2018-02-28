using System;
using System.ComponentModel;

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
            TypeConverter tc = TypeDescriptor.GetConverter(t);
            return tc.ConvertFrom(value);
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
