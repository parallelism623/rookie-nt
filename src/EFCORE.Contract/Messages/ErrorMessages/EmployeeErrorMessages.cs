
using EFCORE.Contract.Shared;

namespace EFCORE.Contract.Messages.ErrorMessages;

public static class EmployeeErrorMessages
{
    public const string NotFound = "Employee not found";
    public const string DepartmentNotFound = "Department not found";
    public const string ProjectNotFound = "Project not found";  
}

public static class EmployeeError
{
    public static Error ProjectNotFound { get; set; } = new Error("ProjectNotFound", EmployeeErrorMessages.ProjectNotFound);
    public static Error NotFound { get;set; } = new Error("EmployeeNotFound", EmployeeErrorMessages.NotFound);

    public static Error DepartmentNotFound { get; set; } = new Error("DepartmentNotFound", EmployeeErrorMessages.DepartmentNotFound);
}
