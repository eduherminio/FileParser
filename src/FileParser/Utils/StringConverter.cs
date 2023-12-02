using System.ComponentModel;

namespace FileParser
{
    internal static class StringConverter
    {
        /// <summary>
        /// Converts strings to basic, nullable types
        /// Optional parameter: an already known typeConverter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <param name="typeConverter"></param>
        /// <returns></returns>
        public static T Convert<T>(string str, TypeConverter? typeConverter = null)
        {
            if (str?.Length == 0)
            {
                return (T)(object)default;
            }
            return default(T) switch
            {
                short => (T)(object)short.Parse(str),
                ushort => (T)(object)ushort.Parse(str),
                int => (T)(object)int.Parse(str),
                uint => (T)(object)uint.Parse(str),
                long => (T)(object)long.Parse(str),
                ulong => (T)(object)ulong.Parse(str),
                // double => (T)(object)double.Parse(str),   // Causes issues with commas and dots separators, since it doesn't use the logic in TConverter.ParseDouble();
                _ => GenericConvert<T>(str, typeConverter)
            };
        }

        internal static T GenericConvert<T>(string str, TypeConverter? typeConverter = null)
        {
            if (typeof(T).IsPrimitive)
            {
                return typeConverter is null
                    ? TConverter.ChangeType<T>(str)
                    : TConverter.ChangeType<T>(str, typeConverter);
            }
            else // Avoids exception if T is an object
            {
                object o = str;
                return (T)o;
            }
        }

        public static TypeConverter GetConverter<T>()
        {
            if (typeof(T).IsPrimitive)
            {
                return TConverter.GetTypeConverter(typeof(T));
            }
            else
            {
                throw new NotSupportedException($"Converter for {typeof(T).Name} yet to be implemented");
            }
        }
    }
}
