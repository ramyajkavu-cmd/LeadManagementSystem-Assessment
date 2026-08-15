using LeadManagementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeadManagementSystem.Controllers.Api;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardApiController : ControllerBase
{
    private readonly AppDbContext _db;

    public DashboardApiController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var leads = await _db.Leads.AsNoTracking().ToListAsync();

        return Ok(new
        {
            totalLeads = leads.Count,
            newLeads = leads.Count(x => x.Status == "New"),
            proposalSent = leads.Count(x => x.Status == "Proposal Sent"),
            won = leads.Count(x => x.Status == "Won"),
            lost = leads.Count(x => x.Status == "Lost"),
            potentialBusinessValue = leads.Where(x => x.Status != "Lost")
                                           .Sum(x => x.EstimatedValue ?? 0),
            byStatus = leads.GroupBy(x => x.Status)
                            .Select(g => new { status = g.Key, count = g.Count() })
        });
    }
}
