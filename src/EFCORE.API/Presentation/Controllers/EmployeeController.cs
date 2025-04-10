using Asp.Versioning;
using EFCORE.Application.UseCases.Employee;
using EFCORE.Contract.Shared;
using Microsoft.AspNetCore.Mvc;

namespace EFCORE.API.Presentation.Controllers;

[Route("v{v:apiVersion}/[controller]")]
public class EmployeeController : ApiBaseController
{
    private readonly IEmployeeService _employeeService;
    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await _employeeService.GetByIdAsync(id);
        return ProcessResult(result);
    }
    [HttpGet("{id:guid}/details")]
    public async Task<IActionResult> GetDetailsByIdAsync(Guid id)
    {
        var result = await _employeeService.GetDetailsByIdAsync(id);
        return ProcessResult(result);
    }
    [MapToApiVersion(1)]
    [HttpGet]
    public async Task<IActionResult> GetWithDepartmentAsync([FromQuery] QueryParameters queryParamerters)
    {
        var result = await _employeeService.GetWithDepartmentAsync(queryParamerters);
        return ProcessResult(result);
    }
    [MapToApiVersion(1)]
    [HttpGet]
    public async Task<IActionResult> GetWithProjectsAsync([FromQuery] QueryParameters queryParamerters)
    {
        var result = await _employeeService.GetWithProjectsAsync(queryParamerters);
        return ProcessResult(result);
    }
    [MapToApiVersion(1)]
    [HttpGet]
    public async Task<IActionResult> GetByJoinedDateAndSalary([FromQuery] QueryParameters queryParamerters)
    {
        var result = await _employeeService.GetByJoinedDateAndSalary(queryParamerters);
        return ProcessResult(result);
    }
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] EmployeeCreateRequest employeeCreateRequest)
    {
        var result = await _employeeService.CreateAsync(employeeCreateRequest);
        return ProcessResult(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] EmployeeUpdateRequest employeeUpdateRequest)
    {
        var result = await _employeeService.UpdateAsync(employeeUpdateRequest);
        return ProcessResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await _employeeService.DeleteAsync(id);
        return ProcessResult(result);
    }
}
