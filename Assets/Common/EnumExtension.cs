namespace Rubik.Common
{
    public static class EnumExtension
    {
        public static T ToEnum<T>(this string enumName, T defaultValue) where T : struct
        {
            if (System.Enum.TryParse(enumName, out T value))
            {
                return value;
            }

            return defaultValue;
        }
        
        public static T ToEnum<T>(this string enumName)
        {
            return (T)System.Enum.Parse(typeof(T), enumName);
        }
    }
}
