using Kian.BL.Data;
using Kian.BL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Kian.MVC.Controllers;
public class StudentsController : Controller
{
    private readonly ApplicationDbContext _context;
    public StudentsController(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var students = await _context.Students.ToListAsync();
        return View(students);

    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();

    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,Phone,DateOfBirth,EnrollmentDate")] Student student)
    {
        if (ModelState.IsValid)
        {
            try
            {

                _context.Add(student);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "تم اضافة الطالب بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "ارجع يا بني ادم ");
            }
        }
        return View(student);
    }
    //(form)ViewModel
    public async Task<IActionResult> Details(int? id)
    {
        if (!id.HasValue)
            return NotFound();

        var student = await _context.Students
                        .Include(s => s.Enrollments)
                        .ThenInclude(e => e.Course)

            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null)
            return NotFound();


        return View(student);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (!id.HasValue)
            return NotFound();

        var student = await _context.Students
            .Include(s => s.Enrollments)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null)
            return NotFound();

        return View(student);

    }

    [HttpPost]
    public async Task<IActionResult> Edit([Bind("Id,FirstName,LastName,Email,Phone,DateOfBirth,EnrollmentDate")] Student student)
    {


        if (ModelState.IsValid)
        {
            if (student.Id < 1)
                return NotFound();
            try
            {
                _context.Update(student);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "تم تحديث بيانات الطالب بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Students.AnyAsync(s => s.Id == student.Id))
                    return NotFound();
                else
                    ModelState.AddModelError("", "حدث خطأ غير متوقع حاول مرة اخرى");
            }
            catch
            {
                ModelState.AddModelError("", "حدث خطأ غير متوقع حاول مرة اخرى");
            }

        }
        return View(student);
    }

    public async Task<IActionResult> Delete(int id)
    {
        if (id < 1)
            return NotFound();

        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null)
            return NotFound();

        return View(student);

    }
    [HttpPost]
    public async Task<IActionResult> Delete([Bind("Id")] Student student)
    {

        if (student.Id < 1)
            return NotFound();
        try
        {

            var deletedStudent = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == student.Id);

            if (student == null)
                return NotFound();

            _context.Remove(deletedStudent!);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "تم حذف بيانات الطالب بنجاح";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            ModelState.AddModelError("", "حدث خطأ غير متوقع حاول مرة اخرى");
        }

        return View(student);

    }

}
