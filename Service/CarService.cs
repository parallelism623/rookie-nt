using System.Runtime.Serialization;

public class CarService
{
    private readonly int MinYear = 1986;
    private readonly int MaxYear = DateTime.Now.Year;
    private readonly List<string> TypeOfCar = new List<string> { "F", "G" };
    const string LastMaintenanceDateFormat = "yyyy-MM-dd";
    const string DateTimeReplenishPowerFormat = "yyyy-MM-dd HH:mm";
    private event Action<DateTime>? ReplenishPowerEvent;
    private delegate void ValidateData(string data);
    public string GetCarModel()
    {
        return GetInputData(Message.NotificationEnterCarModel);
    }
    public string GetCarMaker()
    {
        return GetInputData(Message.NotificationEnterCarMake);
    }
    public string GetTypeOfCar()
    {
        return GetInputData(Message.NotificationChooseTypeOfCar, ValidateTypeOfCar);
    }
    public int GetCarProductionYear()
    {
        var carYear = GetInputData(Message.NotificationEnterCarYear, ValidateCarProductionYear);
        return int.Parse(carYear);

    }
    public DateTime GetCarLastMaintenanceDate()
    {
        var lastMaintenanceDate = GetInputData(string.Format(Message.NotificationEnterLastMaintenanceDateTime, LastMaintenanceDateFormat), ValidateLastMaintenanceDate);
        return DateTimeHelper.ConvertDateTimeFromString(lastMaintenanceDate, LastMaintenanceDateFormat);
    }
    public void HandleReplenishPower(int year)
    {
        Console.Write(Message.NotificationChooseFuelChargeOrNot);
        var isReplenishPower = Console.ReadLine();
        if (isReplenishPower == "N")
        {
            return;
        }
        var dateTimeReplenishPower = new DateTime();
        while (true)
        {
            var input = GetInputData(string.Format(Message.NotificationEnterDateTimeFuelOrCharge, DateTimeReplenishPowerFormat), ValidateDateTimeReplenishPower);
            dateTimeReplenishPower = DateTimeHelper.ConvertDateTimeFromString(input, DateTimeReplenishPowerFormat);
            if (dateTimeReplenishPower.Year >= year && dateTimeReplenishPower.Year <= MaxYear)
            {
                break;
            }
            else
            {
                Console.WriteLine(string.Format(ExceptionMessage.CarYearInvalidValue, year));
            }
        }

        if (ReplenishPowerEvent != null)
            ReplenishPowerEvent.Invoke(dateTimeReplenishPower);
    }


    public void CreateCar()
    {
        var make = GetCarMaker();
        var model = GetCarModel();
        var year = GetCarProductionYear();
        var lastMaintenanceDate = new DateTime();
        while (true)
        {
            lastMaintenanceDate = GetCarLastMaintenanceDate();
            if (DateTimeHelper.GetYear(lastMaintenanceDate) >= year && DateTimeHelper.GetYear(lastMaintenanceDate) <= MaxYear)
            {
                break;
            }
            else
            {
                Console.WriteLine(string.Format(ExceptionMessage.CarYearInvalidValue, year));
            }
        }
        var typeOfCar = GetTypeOfCar();
        if (typeOfCar == "F")
        {
            var fuelCar = new FuelCar(make, model, year, lastMaintenanceDate);
            fuelCar.DisplayDetails();
            ReplenishPowerEvent += fuelCar.Refuel;
        }
        else
        {
            var electricCar = new ElectricCar(make, model, year, lastMaintenanceDate);
            electricCar.DisplayDetails();
            ReplenishPowerEvent += electricCar.Charge;
        }
        HandleReplenishPower(year);
    }
    private void ValidateDateTimeReplenishPower(string dateTimeReplenishPower)
    {
        if (!DateTimeHelper.TryConvertDateTimeFromString(dateTimeReplenishPower, DateTimeReplenishPowerFormat, out _))
        {
            throw new Exception(ExceptionMessage.DateTimeFormatInvalid);
        }
    }
    private void ValidateLastMaintenanceDate(string lastMaintenanceDateString)
    {
        if (!DateTimeHelper.TryConvertDateTimeFromString(lastMaintenanceDateString, LastMaintenanceDateFormat, out _))
        {
            throw new Exception(ExceptionMessage.DateTimeFormatInvalid);
        }
    }
    private void ValidateTypeOfCar(string typeOfCar)
    {
        if (!TypeOfCar.Contains(typeOfCar))
        {
            throw new Exception(ExceptionMessage.InvalidOptionTypeOfCar);
        }
    }
    private void ValidateCarProductionYear(string carYear)
    {
        var invalidYearExceptionMessage = string.Format(ExceptionMessage.CarYearInvalidValue, MinYear);
        if (carYear.StartsWith("0"))
        {
            throw new Exception(invalidYearExceptionMessage);
        }
        if (!int.TryParse(carYear, out int year))
        {
            throw new Exception(invalidYearExceptionMessage);
        }
        if (year < MinYear || year > MaxYear)
        {
            throw new Exception(invalidYearExceptionMessage);
        }
    }
    private string GetInputData(string notificationMessage = default!, ValidateData? validate = null)
    {
        while (true)
        {
            try
            {
                if (!string.IsNullOrEmpty(notificationMessage))
                {
                    Console.Write(notificationMessage);
                }
                var input = Console.ReadLine()?.Trim() ?? throw new Exception(ExceptionMessage.InputShouldNotBeEmpty);
                if (validate != null)
                    validate(input);
                return input;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                continue;
            }
        }
    }
}