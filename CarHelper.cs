public static class CarHelper
{
    public static Car InputNewCar()
    {
        Output(GetMessageInputCarType());
        var type = Input();
        Output(Message.EnterMakeOfCar);
        var make = Input();
        Output(Message.EnterModelOfCar);
        var model = Input();
        Output(Message.YearOfProductionCar);
        int year;
        int.TryParse(Input(), out year);
        if(!ValidateInfoOfCar(make, model, year, type))
        {
            throw new Exception("Data input invalid!");
        }
        var car = new Car(make, model, year, type);
        
        return car;
    }

    public static IEnumerable<Car> InitialData()
    {
        return  new List<Car>
        {
            new Car("Tesla", "Model S", 2022, "Electric"),
            new Car("Toyota", "Corolla", 2021, "Fuel"),
            new Car("Ford", "Mustang", 2020, "Fuel"),
            new Car("Chevrolet", "Bolt EV", 2023, "Electric"),
            new Car("Nissan", "Leaf", 2021, "Electric"),
            new Car("BMW", "i3", 2022, "Electric"),
            new Car("Audi", "Q7", 2021, "Fuel"),
            new Car("Honda", "Civic", 2020, "Fuel"),
            new Car("Hyundai", "Kona Electric", 2022, "Electric"),
            new Car("Mercedes-Benz", "S-Class", 2023, "Fuel")
        };
    }
    private static void Output(string message)
    {
        Console.WriteLine("Output");
    }
    
    private static string Input()
    {
        return Console.ReadLine() ?? throw new ArgumentNullException("Input should not be empty!");
    }

    private static string GetMessageInputCarType()
    {
        return $"{Message.EnterCarType} (Electric/Fuel):";
    }

    private static bool ValidateInfoOfCar(string make, string model, int year, string type)
    {
        return true;
    }
    
}