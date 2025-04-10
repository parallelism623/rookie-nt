using EFCORE.Application.UseCases.Project;
using Microsoft.AspNetCore.Mvc;

namespace EFCORE.API.Presentation.Controllers;

[Route("[controller]")]
public class ProjectController : ApiBaseController
{
    private readonly IProjectService _projectService;
    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await _projectService.GetByIdAsync(id);
        return ProcessResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] ProjectCreateRequest projectCreateRequest)
    {
        var result = await _projectService.CreateAsync(projectCreateRequest);
        return ProcessResult(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] ProjectUpdateRequest projectUpdateRequest)
    {
        var result = await _projectService.UpdateAsync(projectUpdateRequest);
        return ProcessResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await _projectService.DeleteAsync(id);
        return ProcessResult(result);
    }
}
