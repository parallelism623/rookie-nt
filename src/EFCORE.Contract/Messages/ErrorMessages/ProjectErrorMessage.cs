
using EFCORE.Contract.Shared;

namespace EFCORE.Contract.Messages.ErrorMessages;

public static class ProjectErrorMessage
{
    public const string NotFound = "Project not found";

    public const string EmployeeIdsNotExists = "Employee not exists";
}

public static class ProjectError
{
    public static Error NotFound {get;set;} = 
        new Error(
                nameof(ProjectErrorMessage.NotFound), 
                ProjectErrorMessage.NotFound
            );
    public static Error EmployeeIdsNotExists { get; set; } =
        new Error(
                "Bad request",
                ProjectErrorMessage.EmployeeIdsNotExists
            );
}
