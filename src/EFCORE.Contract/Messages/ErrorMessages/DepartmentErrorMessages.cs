
using EFCORE.Contract.Shared;

namespace EFCORE.Contract.Messages.ErrorMessages;

public static class DepartmentErrorMessages
{
    public const string NotFound = "Department not found.";

    public const string EmployeesExsist = "Department has employees, cannot be deleted.";
}

public static class DepartmentError
{
    public static Error NotFound { get;set;} = new Error("Not found", DepartmentErrorMessages.NotFound);

    public static Error EmployeeExists { get; set; } = new Error("Bad request", DepartmentErrorMessages.EmployeesExsist);
}
