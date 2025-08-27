using Kian.API.Models.Students;
using Kian.BL.Data;
using Kian.BL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Kian.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{

    private readonly ApplicationDbContext _context;
    public StudentsController(ApplicationDbContext context)
    {
        _context = context;
    }
    // GET: api/Students
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetAll()
    {
        //some caching can be added here
        //check if data exists in cache
        //if exists return from cache
        //else get from database and store in cache

        var students = await _context.Students.ToListAsync();
        //cache the data for future requests cacheStudents = students
        return Ok(students);
    }
    // GET: api/Students/2
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid ID");

        var students = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
        if (students == null)
            return NotFound();

        return Ok(students);
    }

    // POST: api/Students
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStudentDto student)
    {
        if (student == null)
            return BadRequest("Student is null");
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var ExistsEmail = await _context.Students.AnyAsync(s => s.Email == student.Email);
            if (ExistsEmail)
                return Conflict("Email already exists");

            var newStudent = new Student
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                Phone = student.Phone,
                DateOfBirth = student.DateOfBirth,
                EnrollmentDate = student.EnrollmentDate

            };

            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();

            return Ok(newStudent);
        }
        catch (Exception ex)
        {
            // Log the exception (not implemented here)
            return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new student record");
        }
    }

    // PUT: api/Students/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStudentDto student)
    {
        if (id <= 0 || student == null )
            return BadRequest("Invalid data");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingStudent = await _context.Students.FindAsync(id);
        if (existingStudent == null)
            return NotFound();
        try
        {
            var ExistsEmail = await _context.Students.AnyAsync(s => s.Email == student.Email && s.Id!= id );
            if (ExistsEmail)
                return Conflict("Email already exists");
            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.Email = student.Email;
            existingStudent.Phone = student.Phone;
            existingStudent.DateOfBirth = student.DateOfBirth;

            _context.Entry(existingStudent).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(existingStudent);
        }
        catch (Exception ex)
        {
            // Log the exception (not implemented here)
            return StatusCode(StatusCodes.Status500InternalServerError, "Error updating student record");
        }
    }

    // DELETE: api/Students/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid ID");
        var existingStudent = await _context.Students.FindAsync(id);
        if (existingStudent == null)
            return NotFound();
        try
        {
            _context.Students.Remove(existingStudent);
            await _context.SaveChangesAsync();
            return Ok(new { message = "deleted successfully "});
        }
        catch (Exception ex)
        {
            // Log the exception (not implemented here)
            return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting student record");
        }
    }

    //GET : api/Students/search?query=John
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query is empty");
        var students = await _context.Students
            .Where(s => s.FirstName.Contains(query) ||
            s.LastName.Contains(query) ||
            s.Phone.Contains(query) ||
            s.Email.Contains(query))
            .ToListAsync();
        return Ok(students);
    }
}
