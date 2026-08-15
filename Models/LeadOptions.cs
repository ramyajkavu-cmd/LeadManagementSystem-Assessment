namespace LeadManagementSystem.Models;

public static class LeadOptions
{
    public static readonly string[] Services =
    {
        "Website Development", "Web Application", "Mobile Application",
        "E-Commerce", "SEO", "Digital Marketing", "Other"
    };

    public static readonly string[] Sources =
    {
        "Website", "WhatsApp", "Referral", "LinkedIn", "Google", "Facebook", "Other"
    };

    public static readonly string[] Statuses =
    {
        "New", "Contacted", "Proposal Sent", "Negotiation", "Won", "Lost"
    };

    public static readonly string[] FollowUpTypes =
    {
        "Call", "Email", "Meeting", "WhatsApp", "Demo", "Other"
    };

    public static readonly string[] Assignees =
    {
        "Admin", "Sales Executive", "Business Development Executive"
    };
}
