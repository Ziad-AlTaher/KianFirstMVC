using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kian.BL.Models;
public class Course
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    public int Credits { get; set; }

    [Required]
    [StringLength(50)]
    public string CourseCode { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public bool IsActive { get; set; } = true;

    // Navigation property
    public virtual List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}