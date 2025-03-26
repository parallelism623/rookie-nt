public class Car{
    public Car(string make, string model, int year, string type)
    {
        Make = make;
        Model = model;
        Year = year;
        Enum.TryParse(type, out CarType enumType);
        Type = enumType;
    }
    public string Make {get;set;} = default!;
    public string Model {get;set;} = default!;
    public int Year{get;set;} = default!;
    public CarType Type {get;set; } = default!;
    public override string ToString()
    {
        return $"Car information: Make: {Make}, Model: {Model}, Year: {Year}, Type: {Type}";
    }
}
