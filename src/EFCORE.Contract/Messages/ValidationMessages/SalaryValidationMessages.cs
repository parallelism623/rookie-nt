
namespace EFCORE.Contract.Messages.ValidationMessages;

public static class SalaryValidationMessages
{
    public const string AmountRequired = "Amount is required.";
    public const string IdRequired = "Id is required";
    public const string EmployeeIdRequired = "Employee ID is required.";
    public const string AmountInvalidFormat = "Amount must be a valid decimal number.";
    public const string AmountInvalidRange = "Amount must be greater than 0.";
}
