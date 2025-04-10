using EFCORE.Application.UseCases.Department;
using Microsoft.AspNetCore.Mvc;

namespace EFCORE.API.Presentation.Controllers;

[Route("[controller]")]
public class DepartmentController : ApiBaseController
{
    private readonly IDepartmentService _service;
    public DepartmentController(IDepartmentService service)
    {
        _service = service;
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await _service.GetByIdAsync(id);   
        return ProcessResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] DepartmentCreateRequest departmentCreateRequest)
    {
        var result = await _service.CreateAsync(departmentCreateRequest);
        return ProcessResult(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] DepartmentUpdateRequest departmentUpdateRequest)
    {
        var result = await _service.UpdateAsync(departmentUpdateRequest);
        return ProcessResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await _service.DeleteAsync(id);
        return ProcessResult(result);
    }
}
