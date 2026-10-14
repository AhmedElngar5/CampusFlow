using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentManagementPortfolio.Models;

namespace StudentManagementPortfolio.Data;

public class SchoolDbContext(DbContextOptions<SchoolDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<Student> Students => Set<Student>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Student>()
            .HasIndex(student => student.Email)
            .IsUnique();

        modelBuilder.Entity<Student>().HasData(
            new Student { Id = 1, FullName = "Mariam Hassan", Email = "mariam@example.com", PhoneNumber = "01000000001", Department = "Computer Science", Year = 3, Status = StudentStatus.Active, EnrollmentDate = new DateTime(2024, 9, 15) },
            new Student { Id = 2, FullName = "Omar Adel", Email = "omar@example.com", PhoneNumber = "01000000002", Department = "Information Systems", Year = 2, Status = StudentStatus.Active, EnrollmentDate = new DateTime(2025, 9, 20) },
            new Student { Id = 3, FullName = "Nour Ahmed", Email = "nour@example.com", PhoneNumber = "01000000003", Department = "Artificial Intelligence", Year = 4, Status = StudentStatus.Graduated, EnrollmentDate = new DateTime(2022, 9, 10) },
            new Student { Id = 4, FullName = "Youssef Samir", Email = "youssef@example.com", PhoneNumber = "01000000004", Department = "Computer Science", Year = 1, Status = StudentStatus.Suspended, EnrollmentDate = new DateTime(2026, 9, 25) }
        );
    }
}
