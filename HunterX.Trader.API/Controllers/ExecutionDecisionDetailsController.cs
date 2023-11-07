using HunterX.Trader.Application.Managers;
using Microsoft.AspNetCore.Mvc;

namespace HunterX.Trader.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExecutionDecisionDetailsController : ControllerBase
{
    private readonly ExecutionDecisionManager executionDecisionManager;

    public ExecutionDecisionDetailsController(ExecutionDecisionManager executionDecisionManager)
    {
        this.executionDecisionManager = executionDecisionManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetBasics(int pageNumber, int pageSize)
    {
        var paginatedBasics = await this.executionDecisionManager.GetExecutionDecisionBasicsAsync(pageNumber, pageSize);

        return Ok(paginatedBasics);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var details = await this.executionDecisionManager.GetExecutionDecisionDetailsAsync(id);

        return Ok(details);
    }
}
