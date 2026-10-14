using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementPortfolio.Data;
using StudentManagementPortfolio.Models;

namespace StudentManagementPortfolio.Controllers;

[Authorize]
public class DashboardController(SchoolDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var now = DateTime.Today;
        var firstOfMonth = new DateTime(now.Year, now.Month, 1);

        var model = new DashboardViewModel
        {
            TotalStudents = await context.Students.CountAsync(),
            ActiveStudents = await context.Students.CountAsync(s => s.Status == StudentStatus.Active),
            GraduatedStudents = await context.Students.CountAsync(s => s.Status == StudentStatus.Graduated),
            SuspendedStudents = await context.Students.CountAsync(s => s.Status == StudentStatus.Suspended),
            DepartmentCount = await context.Students.Select(s => s.Department).Distinct().CountAsync(),
            EnrolledThisMonth = await context.Students.CountAsync(s => s.EnrollmentDate >= firstOfMonth),
            DepartmentStats = await context.Students
                .GroupBy(s => s.Department)
                .Select(g => new DepartmentStat { Department = g.Key, Count = g.Count() })
                .OrderByDescending(d => d.Count)
                .ToListAsync(),
            RecentStudents = await context.Students
                .OrderByDescending(s => s.EnrollmentDate)
                .ThenByDescending(s => s.Id)
                .Take(5)
                .ToListAsync()
        };

        return View(model);
    }
}
