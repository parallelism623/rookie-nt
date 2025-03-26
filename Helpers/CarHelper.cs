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
        var yearString = Input();
        ValidateInfoOfCar(make, model, yearString, type);
        int year;
        int.TryParse(yearString, out year);
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
        Console.WriteLine(message);
    }
    
    private static string Input()
    {
        return Console.ReadLine() ?? throw new ArgumentNullException(ExceptionMessage.InputShouldNotBeEmpty);
    }

    private static string GetMessageInputCarType()
    {
        return $"{Message.EnterCarType} (Electric/Fuel):";
    }
    public static void ValidateMakeOfCar(string make)
    {
        if(string.IsNullOrEmpty(make))
        {
            throw new Exception(ExceptionMessage.MakeCarShouldNotBeEmpty);
        }
    }
    public static void ValidateModelOfCar(string model)
    {
        if(string.IsNullOrEmpty(model))
        {
            throw new Exception(ExceptionMessage.ModelCarShouldNotBeEmpty);
        }
    }
    public static void ValidateYearOfCar(string yearString)
    {
        if(string.IsNullOrEmpty(yearString))
        {
            throw new Exception(ExceptionMessage.YearCanNotBeEmpty);
        }
        if(yearString[0] == '-' || yearString[0] == '0')
        {
            throw new Exception(ExceptionMessage.YearInvalid);
        }
        if(!int.TryParse(yearString, out _))
        {
            throw new Exception(ExceptionMessage.YearInvalid);
        }

    }
    public static void ValidateTypeOfCar(string type)
    {
        if(!EnumHelper.IsDefined<CarType>(type))
        {
            throw new Exception(ExceptionMessage.CarTypeNotInvalid);
        }
    }
    private static void ValidateInfoOfCar(string make, string model, string yearString, string type)
    {
        ValidateMakeOfCar(make);
        ValidateYearOfCar(yearString);
        ValidateModelOfCar(model);
        ValidateTypeOfCar(type);
    }
    
}