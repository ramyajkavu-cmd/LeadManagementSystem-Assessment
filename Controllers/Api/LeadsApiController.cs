using LeadManagementSystem.Data;
using LeadManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeadManagementSystem.Controllers.Api;

[ApiController]
[Route("api/leads")]
[Authorize]
public class LeadsApiController : ControllerBase
{
    private readonly AppDbContext _db;

    public LeadsApiController(AppDbContext db) => _db = db;

    [HttpGet]
    [HttpGet]
    public async Task<IActionResult> GetAll(
    string? search = null,
    string? status = null,
    string? service = null,
    string? assignedTo = null,
    string sortBy = "date",
    bool desc = true,
    int page = 1,
    int pageSize = 10)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _db.Leads.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();

            query = query.Where(x =>
                x.LeadName.Contains(search) ||
                x.CompanyName.Contains(search) ||
                x.Email.Contains(search) ||
                x.Mobile.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(x => x.Status == status);

        if (!string.IsNullOrWhiteSpace(service))
            query = query.Where(x => x.ServiceRequired == service);

        if (!string.IsNullOrWhiteSpace(assignedTo))
            query = query.Where(x => x.AssignedTo == assignedTo);

        // IMPORTANT:
        // Count before applying OrderBy.
        var total = await query.CountAsync();

        var sort = sortBy?.Trim().ToLowerInvariant();

        switch (sort)
        {
            case "value":
                // SQLite cannot ORDER BY decimal directly.
                // Convert the value to double for sorting.
                query = desc
                    ? query.OrderByDescending(x => (double?)x.EstimatedValue)
                    : query.OrderBy(x => (double?)x.EstimatedValue);
                break;

            case "name":
                query = desc
                    ? query.OrderByDescending(x => x.LeadName)
                    : query.OrderBy(x => x.LeadName);
                break;

            default:
                query = desc
                    ? query.OrderByDescending(x => x.CreatedDate)
                    : query.OrderBy(x => x.CreatedDate);
                break;
        }

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new
        {
            items,
            total,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling(total / (double)pageSize)
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var lead = await _db.Leads
            .Include(x => x.FollowUps.OrderByDescending(f => f.FollowUpDate))
            .FirstOrDefaultAsync(x => x.Id == id);

        return lead == null ? NotFound(new { message = "Lead not found." }) : Ok(lead);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Lead model)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        if (await _db.Leads.AnyAsync(x => x.Email == model.Email || x.Mobile == model.Mobile))
            return Conflict(new { message = "A lead with the same email or mobile already exists." });

        model.Id = 0;
        model.CreatedDate = DateTime.UtcNow;
        _db.Leads.Add(model);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = model.Id }, model);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Lead model)
    {
        if (id != model.Id) return BadRequest(new { message = "ID mismatch." });
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var lead = await _db.Leads.FindAsync(id);
        if (lead == null) return NotFound(new { message = "Lead not found." });

        lead.LeadName = model.LeadName;
        lead.CompanyName = model.CompanyName;
        lead.Mobile = model.Mobile;
        lead.Email = model.Email;
        lead.ServiceRequired = model.ServiceRequired;
        lead.LeadSource = model.LeadSource;
        lead.EstimatedValue = model.EstimatedValue;
        lead.AssignedTo = model.AssignedTo;
        lead.Remarks = model.Remarks;
        lead.Status = model.Status;

        await _db.SaveChangesAsync();
        return Ok(lead);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var lead = await _db.Leads.FindAsync(id);
        if (lead == null) return NotFound(new { message = "Lead not found." });

        _db.Leads.Remove(lead);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
