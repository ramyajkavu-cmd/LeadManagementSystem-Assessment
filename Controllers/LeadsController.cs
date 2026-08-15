using LeadManagementSystem.Data;
using LeadManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeadManagementSystem.Controllers;

[Authorize]
public class LeadsController : Controller
{
    private readonly AppDbContext _db;

    public LeadsController(AppDbContext db) => _db = db;

    public IActionResult Index() => View();

    public async Task<IActionResult> Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Lead model)
    {
        if (!ModelState.IsValid) return View(model);

        var duplicate = await _db.Leads.AnyAsync(x =>
            x.Email == model.Email || x.Mobile == model.Mobile);

        if (duplicate)
        {
            ModelState.AddModelError("", "A lead with the same email or mobile already exists.");
            return View(model);
        }

        model.CreatedDate = DateTime.UtcNow;
        _db.Leads.Add(model);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var lead = await _db.Leads.FindAsync(id);
        return lead == null ? NotFound() : View(lead);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Lead model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);

        var lead = await _db.Leads.FindAsync(id);
        if (lead == null) return NotFound();

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
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var lead = await _db.Leads
            .Include(x => x.FollowUps.OrderByDescending(f => f.FollowUpDate))
            .FirstOrDefaultAsync(x => x.Id == id);

        return lead == null ? NotFound() : View(lead);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var lead = await _db.Leads.FindAsync(id);
        if (lead == null) return NotFound();

        _db.Leads.Remove(lead);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
