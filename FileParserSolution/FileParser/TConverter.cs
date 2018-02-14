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
        static internal T ChangeType<T>(object value)
        {
            return (T)ChangeType(typeof(T), value);
        }

        static internal object ChangeType(Type t, object value)
        {
            TypeConverter tc = TypeDescriptor.GetConverter(t);
            return tc.ConvertFrom(value);
        }

        static internal void RegisterTypeConverter<T, TC>() where TC : System.ComponentModel.TypeConverter
        {
            TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TC)));
        }
    }
}
