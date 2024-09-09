using EduViz.Entities;
using EduViz.Enums;
using EduViz.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EduViz.Data;

public interface IDataInitialiser
{
    Task InitialiseAsync();
    Task SeedAsync();
    Task TrySeedAsync();
}

public class DatabaseInitialiser : IDataInitialiser
{
    private readonly ApplicationDbContext _context;

    public DatabaseInitialiser(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            // Migration Database - Create database if it does not exist
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        if (_context.Users.Any())
        {
            return;
        }

        var mentor = new User()
        {
            UserId = Guid.NewGuid(),
            Email = "mentor1@gmail.com",
            UserName = "Mentor",
            Password = SecurityUtil.Hash("123456"),
            Role = Role.Mentor,
        };
        var admin = new User()
        {
            UserId = Guid.NewGuid(),
            Email = "admin@gmail.com",
            UserName = "Admin",
            Password = SecurityUtil.Hash("123456"),
            Role = Role.Admin,
        };
        var student = new User()
        {
            UserId = Guid.NewGuid(),
            Email = "student@gmail.com",
            UserName = "Student",
            Password = SecurityUtil.Hash("123456"),
            Role = Role.Student,
        };
        var vipMentor = new User()
        {
            UserId = Guid.NewGuid(),
            Email = "vipmentor@gmail.com",
            UserName = "VipMentor",
            Password = SecurityUtil.Hash("123456"),
            Role = Role.Mentor,
        };
        List<User> users = new List<User>()
        {
            student,
            admin,
            mentor,
            vipMentor
        };
        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();
    }
}
public static class DatabaseInitialiserExtension
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        // Create IServiceScope to resolve service scope
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitialiser>();

        await initializer.InitialiseAsync();

        // Try to seeding data
        await initializer.SeedAsync();
    }
}