public static class DateTimeHelper
{
    public static bool TryConvertDateTimeFromString(string? dateTime, string formatDateTime, out DateTime output)
    {
        if (!DateTime.TryParseExact(dateTime, formatDateTime, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out output))
        {
            return false;
        }
        return true;
    }
    public static DateTime ConvertDateTimeFromString(string dateTime, string formatDateTime)
    {
        return DateTime.ParseExact(dateTime, formatDateTime, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
    }
    public static string GetDateOnly(DateTime dateTime)
    {
        return dateTime.ToString("dd/MM/yyyy");
    }
    public static int GetYear(DateTime dateTime)
    {
        return dateTime.Year;
    }
    public static int GetMonth(DateTime dateTime)
    {
        return dateTime.Month;
    }
    public static int GetDay(DateTime dateTime)
    {
        return dateTime.Day;
    }

}