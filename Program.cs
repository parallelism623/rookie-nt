using System.Reflection;

namespace Program;

public class Program{
    static List<Car> Cars = CarHelper.InitialData().ToList();
    static Dictionary<int,string> choiceWithAction = new Dictionary<int, string>{
                {1, Message.AddACar},
                {2, Message.ViewAllCars},
                {3, Message.SearchCarsByMake},
                {4, Message.FilterCarsByType},
                {5, Message.RemoveACarByModel},
                {6, Message.Exit}
    };
    static bool IsChoiceValid(int choice)
    {
        return choiceWithAction.Keys.Contains(choice);
    }
    static void AddCar()
    {
        try{
            Cars.Add(CarHelper.InputNewCar());
            Console.WriteLine(Message.CarAddedSuccessfully);
        }
        catch(Exception ex){
            Console.WriteLine($"{Message.CarAddedFail}!\nFail by: {ex.Message}");
        }
    }
    private static void PrintCarsInformation(List<Car> cars)
    {
        foreach(var car in cars)
        {
            Console.WriteLine(car);
        }
    }
    private static IEnumerable<Car> GetAllCars()
    {
        return Cars ?? new List<Car>();
    }
    static void ViewAllCars()
    {
        PrintCarsInformation(Cars);
    }
    
    static void SearchCarsByMake()
    {
        Console.WriteLine(Message.EnterMakeOfCar);
        var make = Console.ReadLine()?.Trim();
        CarHelper.ValidateMakeOfCar(make!);
        var cars = GetAllCars().Where(c => c.Make == make).ToList();
        if(cars.Any())
        {
            PrintCarsInformation(cars);
        }
        else{
            Console.WriteLine(Message.CarNotFoundByMake);
        }
    }

    static void FilterCarsByType()
    {
        Console.WriteLine(Message.EnterCarType);
        var type = Console.ReadLine()?.Trim();
        CarHelper.ValidateTypeOfCar(type!);
        Enum.TryParse(type, out CarType result);
        var carsFilterByType = GetAllCars().Where(c => c.Type.ToString().Contains(result.ToString())).ToList();
        
        if(carsFilterByType.Any())
        {
            PrintCarsInformation(carsFilterByType);
        }
        else
        {
            Console.WriteLine(Message.CarNotFoundByType);
        }
    }
    static void RemoveCarByModel()
    {
        Console.WriteLine(Message.EnterModelOfCar);
        var model = Console.ReadLine()?.Trim();
        CarHelper.ValidateModelOfCar(model!);
        var car = GetAllCars().FirstOrDefault(c => c.Model.Contains(model!));
        if(car != null)
        {
            Cars.Remove(car);
            Console.WriteLine(string.Format(Message.CarDeleteSuccess, car));
        }
        else
        {
            Console.WriteLine(Message.CarNotFoundByModel);
        }
    }
    
    private static void PrintMenuOption()
    {
        Console.WriteLine(Message.MenuAction);
        foreach(var key in choiceWithAction.Keys)
        {
            Console.WriteLine($"{key} {choiceWithAction[key]}");
        }
    }

    public static void Main(string[] args){
        while(true)
        {   
            PrintMenuOption();
            try{
                Console.WriteLine(Message.EnterChoice);
                var input = Console.ReadLine()?.Trim() ?? throw new ArgumentNullException(ExceptionMessage.UserChoiceShouldNotEmpty);
                int choice;
                if(!int.TryParse(input, out choice))
                {
                    throw new Exception(ExceptionMessage.ChoiceBeNotNumber);
                };
                if(!IsChoiceValid(choice))
                {
                    Console.WriteLine(ExceptionMessage.ChoiceNotMatchOption);
                    continue;
                }

                switch (choice)
                {
                    case 1: 
                    {
                        AddCar();
                        break;
                    }
                    case 2:
                    {
                        ViewAllCars();
                        break;
                    }
                    case 3:
                    {
                        SearchCarsByMake();
                        break;
                    }
                    case 4:
                    {
                        FilterCarsByType();
                        break;
                    }
                    case 5:
                    {
                        RemoveCarByModel();
                        break;
                    }
                    case 6:
                    {
                        Console.WriteLine("Exit program");
                        return;
                    }
                }

            }
            catch(Exception ex){
                Console.WriteLine(ex.Message);
                continue;
            }
        }
    }
}