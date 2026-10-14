using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementPortfolio.Data;
using StudentManagementPortfolio.Models;

namespace StudentManagementPortfolio.Controllers;

[Authorize]
public class StudentsController(SchoolDbContext context) : Controller
{
    private const int PageSize = 8;

    [HttpGet]
    public async Task<IActionResult> Index(
        string? search, string? department, StudentStatus? status,
        string? sortBy, bool sortDesc = false, int page = 1)
    {
        var query = BuildFilteredQuery(search, department, status);

        var totalCount = await query.CountAsync();
        var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)PageSize));
        page = Math.Clamp(page, 1, totalPages);

        query = ApplySorting(query, sortBy, sortDesc);

        var students = await query
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        var model = new StudentListViewModel
        {
            Students = students,
            Search = search,
            Department = department,
            Status = status?.ToString(),
            Page = page,
            TotalPages = totalPages,
            TotalCount = totalCount,
            Departments = await context.Students.Select(s => s.Department).Distinct().OrderBy(d => d).ToListAsync(),
            SortBy = sortBy,
            SortDesc = sortDesc
        };

        return View(model);
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpGet]
    public IActionResult Create()
    {
        return View(new StudentViewModel { EnrollmentDate = DateTime.Today });
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StudentViewModel model)
    {
        if (await context.Students.AnyAsync(s => s.Email == model.Email))
            ModelState.AddModelError(nameof(model.Email), "This email address is already registered.");

        if (!ModelState.IsValid) return View(model);

        var student = new Student
        {
            FullName = model.FullName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            Department = model.Department,
            Year = model.Year,
            Status = model.Status,
            EnrollmentDate = model.EnrollmentDate
        };

        context.Students.Add(student);
        await context.SaveChangesAsync();
        TempData["Success"] = "Student added successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();
        var student = await context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        return student is null ? NotFound() : View(student);
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();
        var student = await context.Students.FindAsync(id);
        if (student is null) return NotFound();

        var model = new StudentViewModel
        {
            FullName = student.FullName,
            Email = student.Email,
            PhoneNumber = student.PhoneNumber,
            Department = student.Department,
            Year = student.Year,
            Status = student.Status,
            EnrollmentDate = student.EnrollmentDate
        };

        ViewBag.StudentId = student.Id;
        return View(model);
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, StudentViewModel model)
    {
        if (await context.Students.AnyAsync(s => s.Email == model.Email && s.Id != id))
            ModelState.AddModelError(nameof(model.Email), "This email address is already registered.");

        if (!ModelState.IsValid)
        {
            ViewBag.StudentId = id;
            return View(model);
        }

        var student = await context.Students.FindAsync(id);
        if (student is null) return NotFound();

        student.FullName = model.FullName;
        student.Email = model.Email;
        student.PhoneNumber = model.PhoneNumber;
        student.Department = model.Department;
        student.Year = model.Year;
        student.Status = model.Status;
        student.EnrollmentDate = model.EnrollmentDate;

        try
        {
            await context.SaveChangesAsync();
            TempData["Success"] = "Student details updated successfully.";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await context.Students.AnyAsync(s => s.Id == id)) return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();
        var student = await context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        return student is null ? NotFound() : View(student);
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var student = await context.Students.FindAsync(id);
        if (student is not null)
        {
            context.Students.Remove(student);
            await context.SaveChangesAsync();
            TempData["Success"] = "Student deleted successfully.";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Export(string? search, string? department, StudentStatus? status)
    {
        var query = BuildFilteredQuery(search, department, status);
        var students = await query.OrderBy(s => s.FullName).ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("Id,FullName,Email,PhoneNumber,Department,Year,Status,EnrollmentDate");
        foreach (var s in students)
        {
            sb.AppendLine($"{s.Id},\"{Escape(s.FullName)}\",\"{Escape(s.Email)}\",\"{Escape(s.PhoneNumber ?? "")}\",\"{Escape(s.Department)}\",{s.Year},{s.Status},\"{s.EnrollmentDate:yyyy-MM-dd}\"");
        }

        var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
        return File(bytes, "text/csv", $"students-{DateTime.Now:yyyyMMdd-HHmmss}.csv");
    }

    private IQueryable<Student> BuildFilteredQuery(string? search, string? department, StudentStatus? status)
    {
        var query = context.Students.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(s => s.FullName.Contains(term) || s.Email.Contains(term) || s.Department.Contains(term));
        }
        if (!string.IsNullOrWhiteSpace(department))
            query = query.Where(s => s.Department == department);
        if (status.HasValue)
            query = query.Where(s => s.Status == status.Value);

        return query;
    }

    private static IQueryable<Student> ApplySorting(IQueryable<Student> query, string? sortBy, bool sortDesc)
    {
        return sortBy?.ToLowerInvariant() switch
        {
            "name" => sortDesc ? query.OrderByDescending(s => s.FullName) : query.OrderBy(s => s.FullName),
            "department" => sortDesc ? query.OrderByDescending(s => s.Department) : query.OrderBy(s => s.Department),
            "year" => sortDesc ? query.OrderByDescending(s => s.Year) : query.OrderBy(s => s.Year),
            "status" => sortDesc ? query.OrderByDescending(s => s.Status) : query.OrderBy(s => s.Status),
            "date" => sortDesc ? query.OrderByDescending(s => s.EnrollmentDate) : query.OrderBy(s => s.EnrollmentDate),
            _ => query.OrderBy(s => s.FullName)
        };
    }

    private static string Escape(string value) => value.Replace("\"", "\"\"");
}
