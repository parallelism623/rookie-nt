
namespace EFCORE.Contract.Messages.ValidationMessages;

public static class EmployeeValidationMessages
{
    public const string NameRequired = "Employee name is required";
    public const string MaximumLengthName = "Employee name must be at most 100 characters long";
    public const string NameInvalidFormat = "Employee name must contain only letters unicode";
    public const string IdRequired = "Employee ID is required";
    public const string DepartmentIdRequired = "Department ID is required";
    public const string JoinedDateRequired = "Joined date is required";
    public const string AmountInvalid = "Amount must be greater than 0";
}
