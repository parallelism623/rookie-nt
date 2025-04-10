
namespace EFCORE.Contract.Messages.ValidationMessages;

public static class DepartmentValidationMessages
{
    public const string NameRequired = "Department name is required.";
    public const string MaximumLengthName = "Department name must be at most 100 characters long.";
    public const string NameInvalidFormat = "Department name must contain only letters unicode.";

    public const string IdRequired = "Department ID is required.";
}
