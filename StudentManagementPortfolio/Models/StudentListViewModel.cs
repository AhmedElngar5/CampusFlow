namespace StudentManagementPortfolio.Models;

public class StudentListViewModel
{
    public IReadOnlyList<Student> Students { get; set; } = Array.Empty<Student>();
    public string? Search { get; set; }
    public string? Department { get; set; }
    public string? Status { get; set; }
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public List<string> Departments { get; set; } = new();
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; }
}
