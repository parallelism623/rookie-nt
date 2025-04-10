
namespace EFCORE.Contract.Messages.ValidationMessages;

public static class ProjectValidationMessage
{
    public const string NameRequired = "Project name is required.";
    public const string MaximumLengthName = "Project name must be at most 100 characters long.";
    public const string NameInvalidFormat = "Project name must contain only letters unicode.";
    public const string IdRequired = "Project ID is required.";
    public const string DescriptionRequired = "Project description is required.";
    public const string DescriptionInvalidFormat = "Project description must contain only letters unicode.";
}
