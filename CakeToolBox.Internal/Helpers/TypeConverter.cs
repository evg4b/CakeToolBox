namespace CakeToolBox.Internal.Helpers
{
    using System;
    using System.ComponentModel;

    public static class TypeConverter
    {
        public static T ConvertTo<T>(string value) => (T) ConvertTo(typeof(T), value);

        public static object ConvertTo(Type type, string value)
        {
            var converter = TypeDescriptor.GetConverter(type);
            return converter.ConvertFromInvariantString(value);
        }
    }
}