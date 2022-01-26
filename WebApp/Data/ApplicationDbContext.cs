using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Topic> Topics { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        string adminRoleName = "admin";
        string userRoleName = "user";
 
        string adminEmail = "admin@mail.ru";
        string adminPassword = "123456";
 
        // добавляем роли
        Role adminRole = new Role { Id = 1, Name = adminRoleName };
        Role userRole = new Role { Id = 2, Name = userRoleName };
        User adminUser = new User { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id , FirstName = "qwe",LastName = "qwe", Topics = new List<Topic>()};
 
        modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
        modelBuilder.Entity<User>().HasData( new User[] { adminUser });
        modelBuilder.Entity<Topic>().HasData( new Topic[] { new Topic(){TopicId = 1, Name = "Default topic", Info = "None", UserId = 1} });
        base.OnModelCreating(modelBuilder);
    }
}