
using EFCORE.Contract.Shared;

namespace EFCORE.Contract.Messages.ErrorMessages;

public static class SalaryErrorMessages
{
    public const string NotFound = "Salary not found";
    public const string EmployeeIdInvalid = "EmployeeId invalid";
}

public static class SalaryErrors
{
    public static Error NotFound { get; set; } = new Error("SalaryNotFound", SalaryErrorMessages.NotFound);
    public static Error EmployeeIdInvalid { get; set; } = new Error("EmployeeIdInvalid", SalaryErrorMessages.EmployeeIdInvalid);
}
