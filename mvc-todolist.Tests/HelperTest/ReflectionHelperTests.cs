
using FluentAssertions;
using mvc_todolist.Commons.Helpers;
using System.Reflection;

namespace mvc_todolist.Tests.HelperTest
{

    public class BaseClass
    {
        public string InheritedProp { get; set; }
    }

    public class Dummy : BaseClass
    {
        public int PropOne { get; set; }
        public string PropTwo { get; set; }
        public bool PropThree { get; set; }
    }

    public class DummyWithoutInherited
    {
        public int PropA { get; set; }
        public string PropB { get; set; }
    }

    [TestFixture]
    public class ReflectionHelperUnitTests
    {
        public static IEnumerable<TestCaseData> TestCasesGetPropertiesNameOfType()
        {
            yield return new TestCaseData(typeof(Dummy)).Returns(new List<string> { "PropOne", "PropTwo", "PropThree"});
            yield return new TestCaseData(typeof(DummyWithoutInherited)).Returns(new List<string> { "PropA", "PropB" });
        }
        [Test, TestCaseSource(nameof(TestCasesGetPropertiesNameOfType))]
        public IEnumerable<string>? GetPropertiesNameOfType_ShouldReturnOnlyDeclaredProperties(Type t)
        {
            var methodInfo = typeof(ReflectionHelper).GetMethod("GetPropertiesNameOfType");

            var genericMethod = methodInfo?.MakeGenericMethod(t);
            var result = genericMethod?.Invoke(null, null) as IEnumerable<string>;

            return result;
        }

        [Test, TestCaseSource(nameof(TestCasesGetPropertiesNameOfType))]
        public IEnumerable<string>? GetPropertiesNameOfType_ShouldReturnOnlyDeclaredPropertiesInfo(Type t)
        {
            var methodInfo = typeof(ReflectionHelper).GetMethod("GetPropertiesInfoOfType");

            var genericMethod = methodInfo?.MakeGenericMethod(t);
            var result = genericMethod?.Invoke(null, null) as IEnumerable<PropertyInfo>;
            var propertyNames = result?.Select(p => p.Name).ToList();
            return propertyNames;
        }

    }
    

}
