using EFCORE.Application.UseCases.Salary;
using Microsoft.AspNetCore.Mvc;

namespace EFCORE.API.Presentation.Controllers;


[Route("[controller]")]
public class SalaryController : ApiBaseController
{
    private readonly ISalaryService _salaryService;
    public SalaryController(ISalaryService salaryService)
    {
        _salaryService = salaryService;
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await _salaryService.GetByIdAsync(id);
        return ProcessResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] SalaryCreateRequest salaryCreateRequest)
    {
        var result = await _salaryService.CreateAsync(salaryCreateRequest);
        return ProcessResult(result);
    }
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] SalaryUpdateRequest salaryUpdateRequest)
    {
        var result = await _salaryService.UpdateAsync(salaryUpdateRequest);
        return ProcessResult(result);
    }

}
