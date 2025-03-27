public abstract class Car
{

    private readonly List<DateTime> _historyReplenishPower;
    public Car(string make, string model, int year, DateTime lastMaintenanceDate)
    {
        Make = make;
        Model = model;
        Year = year;
        LastMaintenanceDate = lastMaintenanceDate;
        _historyReplenishPower = new List<DateTime>();
    }
    public string Make { get; set; } = default!;
    public string Model { get; set; } = default!;
    public int Year { get; set; } = default!;
    public DateTime LastMaintenanceDate = default!;

    public void DisplayDetails()
    {
        Console.WriteLine(string.Format(Message.CarInfo, Make, Model, Year));
        Console.WriteLine(string.Format(Message.LastMaintenanceInfo, DateTimeHelper.GetDateOnly(LastMaintenanceDate)));
        Console.WriteLine(string.Format(Message.NextMaintenanceInfo, DateTimeHelper.GetDateOnly(ScheduleMaintenance())));
    }
    public DateTime ScheduleMaintenance()
    {
        return LastMaintenanceDate.AddMonths(6);
    }

    public void AddReplenishPowerToHistory(DateTime dateTime)
    {
        _historyReplenishPower.Append(dateTime);
    }

}