using System;
using System.ComponentModel;
using System.Reflection;

namespace RatingsAnalyzer.Util
{
    public static class EnumExtensions
    {
        public static T ConvertToEnum<T>(this string value)
            where T: struct
        {
            var type = typeof (T);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException(String.Format("{0} is not an Enum", type.FullName));
            }
            
            foreach (var field in type.GetFields())
            {
                var description = field.GetCustomAttribute<DescriptionAttribute>();
                if ((field.Name == value) || (description != null && description.Description == value))
                {
                    return (T) field.GetValue(null);
                }
            }
            throw new InvalidOperationException(String.Format("{0} is not a valid value of {1}", value, type.FullName));
        }
    }
}
