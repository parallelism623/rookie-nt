
public class ElectricCar : Car, IChargable
{
    public ElectricCar(string make, string model, int year, DateTime lastMaintenanceDate) : base(make, model, year, lastMaintenanceDate)
    {

    }

    public void Charge(DateTime timeOfCharge)
    {
        Console.WriteLine($"Electric {Make} {Model} charged on {timeOfCharge.ToString("yyyy-MM-dd HH:mm")}");
    }

}
