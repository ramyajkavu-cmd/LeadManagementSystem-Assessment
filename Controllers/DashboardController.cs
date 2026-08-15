using LeadManagementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeadManagementSystem.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly AppDbContext _db;

    public DashboardController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var leads = await _db.Leads.AsNoTracking().ToListAsync();

        ViewBag.TotalLeads = leads.Count;
        ViewBag.NewLeads = leads.Count(x => x.Status == "New");
        ViewBag.ProposalSent = leads.Count(x => x.Status == "Proposal Sent");
        ViewBag.Won = leads.Count(x => x.Status == "Won");
        ViewBag.Lost = leads.Count(x => x.Status == "Lost");
        ViewBag.PotentialValue = leads
            .Where(x => x.Status != "Lost")
            .Sum(x => x.EstimatedValue ?? 0);

        ViewBag.StatusData = leads
            .GroupBy(x => x.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .OrderBy(x => x.Status)
            .ToList();

        return View();
    }
}
