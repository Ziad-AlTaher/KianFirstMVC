using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kian.BL.Models;
public class Student
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    public string? Phone { get; set; }

    public DateTime DateOfBirth { get; set; }

    public DateTime EnrollmentDate { get; set; } = DateTime.Now;

    // Navigation property
    public  List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}