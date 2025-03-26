public class Car{
    public Car(string make, string model, int year, string type)
    {
        Make = make;
        Model = model;
        Year = year;
        Enum.TryParse(type, out Type enumType);
        Type = enumType;
    }
    public string? Make {get;set;}
    public string? Model {get;set;}
    public int? Year{get;set;}
    public Type? Type {get;set; }
    public override string ToString()
    {
        return $"Car information: Make: {Make}, Model: {Model}, Year: {Year}, Type: {Type}";
    }
}
