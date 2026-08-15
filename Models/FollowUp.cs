using System.ComponentModel.DataAnnotations;

namespace LeadManagementSystem.Models;

public class FollowUp
{
    public int Id { get; set; }

    public int LeadId { get; set; }
    public Lead? Lead { get; set; } = null!;

    [Required]
    public DateTime FollowUpDate { get; set; } = DateTime.Today;

    [Required, StringLength(50)]
    public string FollowUpType { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Remarks { get; set; }

    public DateTime? NextFollowUpDate { get; set; }
}
