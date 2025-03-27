public static class Message
{
    public static string NotificationEnterCarMake = "Enter car make: ";

    public static string NotificationEnterCarModel = "Enter car model: ";

    public static string NotificationEnterDateTimeFuelOrCharge = "Enter refuel/charge date and time ({0}): ";

    public static string NotificationChooseFuelChargeOrNot = "Do you want to refuel/charge? (Y/N): ";

    public static string NotificationChooseTypeOfCar = "Is this a FuelCar or ElectricCar? (F/E): ";

    public static string NotificationEnterCarYear = "Enter car year (e.g., 2020): ";

    public static string NotificationEnterLastMaintenanceDateTime = "Enter last maintenance date ({0}): ";

    public static string LastMaintenanceInfo = "Last Maintenance: {0}";
    public static string NextMaintenanceInfo = "Next Maintenance: {0}";

    public static string CarInfo = "Car: {0} {1} ({2})";
}

public static class ExceptionMessage
{
    public static string InputShouldNotBeEmpty = "Input should not be empty. Please try again!";

    public static string CarYearInvalidValue = "Invalid year! Please enter a valid year between {0} and the current year";

    public static string DateTimeFormatInvalid = "Invalid date format! Please enter a valid date";

    public static string InvalidOptionTypeOfCar = "Invalid input! Please enter 'F' for FuelCar or 'E' for ElectricCar";


}