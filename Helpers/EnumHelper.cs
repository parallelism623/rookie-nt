public static class EnumHelper
{
    public static bool IsDefined<T> (string enumString)
    {
        return Enum.IsDefined(typeof(T), enumString);
    }
}