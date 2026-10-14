using System.ComponentModel.DataAnnotations;

namespace StudentManagementPortfolio.Models;

public class StudentViewModel
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(60, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 60 characters.")]
    [Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(120)]
    public string Email { get; set; } = string.Empty;

    [StringLength(20)]
    [Display(Name = "Phone number")]
    [RegularExpression(@"^[0-9+\-\s()]*$", ErrorMessage = "Please enter a valid phone number.")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Department is required.")]
    [StringLength(80)]
    public string Department { get; set; } = string.Empty;

    [Range(1, 8, ErrorMessage = "Year must be between 1 and 8.")]
    [Display(Name = "Academic year")]
    public int Year { get; set; } = 1;

    [Required]
    public StudentStatus Status { get; set; } = StudentStatus.Active;

    [DataType(DataType.Date)]
    [Display(Name = "Enrollment date")]
    public DateTime EnrollmentDate { get; set; } = DateTime.Today;
}
