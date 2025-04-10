using EFCORE.Application.UseCases.ProjectEmployee;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EFCORE.API.Presentation.Controllers;

[Route("[controller]")]
public class ProjectEmployeeController : ApiBaseController
{
    private readonly IProjectEmployeeSerivce _projectEmployeeService;
    public ProjectEmployeeController(IProjectEmployeeSerivce projectEmployeeSerivce)
    {
        _projectEmployeeService = projectEmployeeSerivce;
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await _projectEmployeeService.GetByIdAsync(id);
        return ProcessResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] ProjectEmployeeCreateRequest projectEmployeeCreateRequest)
    {
        var result = await _projectEmployeeService.CreateAsync(projectEmployeeCreateRequest);
        return ProcessResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await _projectEmployeeService.DeleteAsync(id);
        return ProcessResult(result);
    }
}
