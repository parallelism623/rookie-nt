using System.Reflection;

namespace mvc_todolist.Commons.Helpers
{
    public static class ReflectionHelper
    {
        public static List<string> GetPropertiesNameOfType<T>()
        {
            return typeof(T).GetProperties(System.Reflection.BindingFlags.Instance |
                                           System.Reflection.BindingFlags.Public |
                                           System.Reflection.BindingFlags.DeclaredOnly)
                .Select(c => c.Name)
                .ToList();
        }

        public static List<PropertyInfo> GetPropertiesInfoOfType<T>()
        {
            return typeof(T).GetProperties().Where(p => p.DeclaringType == typeof(T)).ToList();
        }
    }
}
