using System.ComponentModel.DataAnnotations;

namespace LeadManagementSystem.Models;

public class Lead
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string LeadName { get; set; } = string.Empty;

    [Required, StringLength(150)]
    public string CompanyName { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Mobile { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string ServiceRequired { get; set; } = string.Empty;

    [Required]
    public string LeadSource { get; set; } = string.Empty;

    [Range(0, 999999999)]
    public decimal? EstimatedValue { get; set; }

    [Required]
    public string AssignedTo { get; set; } = string.Empty;

    public string? Remarks { get; set; }

    [Required]
    public string Status { get; set; } = "New";

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public ICollection<FollowUp> FollowUps { get; set; } = new List<FollowUp>();
}
