namespace StudentManagementPortfolio.Models;

public class DashboardViewModel
{
    public int TotalStudents { get; set; }
    public int ActiveStudents { get; set; }
    public int GraduatedStudents { get; set; }
    public int SuspendedStudents { get; set; }
    public int DepartmentCount { get; set; }
    public int EnrolledThisMonth { get; set; }
    public List<DepartmentStat> DepartmentStats { get; set; } = new();
    public List<Student> RecentStudents { get; set; } = new();
}

public class DepartmentStat
{
    public string Department { get; set; } = string.Empty;
    public int Count { get; set; }
}
