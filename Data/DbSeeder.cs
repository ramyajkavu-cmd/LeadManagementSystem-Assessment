using LeadManagementSystem.Models;
using LeadManagementSystem.Services;

namespace LeadManagementSystem.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.Users.Any()) return;

        var passwordService = new PasswordService();
        db.Users.Add(new AppUser
        {
            Username = "admin",
            PasswordHash = passwordService.HashPassword("Admin@123"),
            DisplayName = "Administrator"
        });

        db.SaveChanges();
    }
}
