
using EFCORE.Contract.Shared;

namespace EFCORE.Contract.Messages.ErrorMessages;

public static class ProjectEmployeeErrorMessages
{
    public const string EmployeeAlreadyExistsInProject = "Employee already exists in project.";
    public const string NotFound = "Project employee not found";
    public const string EmployeeNotFound = "Employee not found";
    public const string ProjectNotFound = "Project not found";
    public const string EmployeeNotExistsInProject = "Employee not exists in project.";
}

public static class ProjectEmployeeErrors
{
    public static Error EmployeeAlreadyExistsInProject { get; set; } = 
        new(nameof(EmployeeAlreadyExistsInProject), ProjectEmployeeErrorMessages.EmployeeAlreadyExistsInProject);

    public static Error NotFound { get; set; } =
        new(nameof(NotFound), ProjectEmployeeErrorMessages.NotFound);

    public static Error EmployeeNotFound { get; set; } =
        new(nameof(EmployeeNotFound), ProjectEmployeeErrorMessages.EmployeeNotFound);

    public static Error ProjectNotFound { get; set; } =
        new(nameof(ProjectNotFound), ProjectEmployeeErrorMessages.ProjectNotFound);

    public static Error EmployeeNotExistsInProject { get; set; } =
        new(nameof(EmployeeNotExistsInProject), ProjectEmployeeErrorMessages.EmployeeNotExistsInProject);
}
