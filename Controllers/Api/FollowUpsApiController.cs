using LeadManagementSystem.Data;
using LeadManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeadManagementSystem.Controllers.Api;

[ApiController]
[Route("api/leads/{leadId:int}/followups")]
[Authorize]
public class FollowUpsApiController : ControllerBase
{
    private readonly AppDbContext _db;

    public FollowUpsApiController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll(int leadId)
    {
        if (!await _db.Leads.AnyAsync(x => x.Id == leadId))
            return NotFound(new { message = "Lead not found." });

        var items = await _db.FollowUps
            .Where(x => x.LeadId == leadId)
            .OrderByDescending(x => x.FollowUpDate)
            .ToListAsync();

        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create(int leadId, [FromBody] FollowUp model)
    {
        if (!await _db.Leads.AnyAsync(x => x.Id == leadId))
            return NotFound(new { message = "Lead not found." });

        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        model.Id = 0;
        model.LeadId = leadId;
        _db.FollowUps.Add(model);
        await _db.SaveChangesAsync();

        return Ok(model);
    }
}
