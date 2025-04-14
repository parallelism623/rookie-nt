
using Microsoft.Extensions.Logging;
using Moq;

namespace mvc_todolist.Tests.Commons.Extensions
{
    public static class LoggerMockExtensions
    {
        public static void VerifyLog<T>(
            this Mock<ILogger<T>> loggerMock,
            LogLevel logLevel,
            string messageTemplate,
            Dictionary<string, object> expectedParams,
            Times times,
            string failMessage = default!)
        {
            loggerMock.Verify(
                l => l.Log(
                    It.Is<LogLevel>(lvl => lvl == logLevel),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, t) => IsValidLogState(state, messageTemplate, expectedParams)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                times,
                failMessage);
        }

        private static bool IsValidLogState(object state, string messageTemplate, Dictionary<string, object> expectedParams)
        {
            var formattedLogValues = state as IReadOnlyList<KeyValuePair<string, object>>;
            if (formattedLogValues == null)
                return false;

            var originalFormat = formattedLogValues.FirstOrDefault(kvp => kvp.Key == "{OriginalFormat}").Value?.ToString();
            if (originalFormat != messageTemplate)
                return false;

            foreach (var expectedParam in expectedParams)
            {
                var actualParam = formattedLogValues.FirstOrDefault(kvp => kvp.Key == expectedParam.Key);
                if (actualParam.Key == null)
                    return false;

                if (expectedParam.Key == "Data" && expectedParam.Value is Dictionary<string, string> expectedDict)
                {
                    var actualDict = actualParam.Value as Dictionary<string, string>;
                    if (actualDict == null || !expectedDict.All(kvp => actualDict.ContainsKey(kvp.Key) && actualDict[kvp.Key] == kvp.Value))
                        return false;
                }
                else if (!Equals(actualParam.Value, expectedParam.Value))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
