using System.ComponentModel.DataAnnotations;

namespace StudentManagementPortfolio.Models;

public class Student
{
    public int Id { get; set; }

    [Required, StringLength(60, MinimumLength = 2)]
    [Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(120)]
    public string Email { get; set; } = string.Empty;

    [StringLength(20)]
    [Display(Name = "Phone number")]
    public string? PhoneNumber { get; set; }

    [Required, StringLength(80)]
    public string Department { get; set; } = string.Empty;

    [Range(1, 8)]
    public int Year { get; set; } = 1;

    [Required]
    public StudentStatus Status { get; set; } = StudentStatus.Active;

    [DataType(DataType.Date)]
    [Display(Name = "Enrollment date")]
    public DateTime EnrollmentDate { get; set; } = DateTime.Today;
}

public enum StudentStatus
{
    Active,
    Graduated,
    Suspended
}
