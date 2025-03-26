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
        catch{
            Console.WriteLine(Message.CarAddedFail);
        }
    }
    static void ViewAllCars()
    {
        foreach(var car in Cars)
        {
            Console.WriteLine(car);
        }
    }

    static IEnumerable<Car> SearchCarsByMake()
    {
        Console.WriteLine(Message.EnterMakeOfCar);
        var make = Console.ReadLine();
        return Cars.Where(c => c.Make == make);
    }

    static IEnumerable<Car> FilterCarsByType()
    {
        Console.WriteLine(Message.EnterCarType);
        var type = Console.ReadLine();
        Enum.TryParse(type, out Type result);
        return Cars.Where(c => c.Type.ToString() == result.ToString());
    }
    static void RemoveCarByModel()
    {
        Console.WriteLine(Message.EnterModelOfCar);
        var model = Console.ReadLine();
        var car = Cars.FirstOrDefault(c => c.Model == model);
        if(car != null)
        {
            Cars.Remove(car);
        }
    }

    public static void Main(string[] args){
        Console.WriteLine(Message.MenuAction);
        foreach(var key in choiceWithAction.Keys)
        {
            Console.WriteLine($"{key} {choiceWithAction[key]}");
        }
        while(true)
        {   
            var input = Console.ReadLine() ?? throw new ArgumentNullException(ExceptionMessage.UserChoiceShouldNotEmpty);
            try{
                var choice = int.Parse(input);
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
                        return;
                    }
                }

            }
            catch{
                Console.WriteLine(ExceptionMessage.ChoiceBeNotNumber);
                continue;
            }
        }
    }
}