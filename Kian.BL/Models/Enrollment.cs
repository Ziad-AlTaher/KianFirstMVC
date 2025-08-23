using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kian.BL.Models;
public class Enrollment
{
    public int Id { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public int CourseId { get; set; }

    public DateTime EnrollmentDate { get; set; } = DateTime.Now;

    [Range(0, 100)]
    public decimal? Grade { get; set; }

    public bool IsCompleted { get; set; } 

    public DateTime? CompletionDate { get; set; }

    // Navigation properties
    public virtual Student Student { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
}