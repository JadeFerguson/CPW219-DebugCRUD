using CPW219_CRUD_Troubleshooting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPW219_CRUD_Troubleshooting.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolContext context;

        public StudentsController(SchoolContext dbContext)
        {
            context = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            List<Student> students = await (from student in context.Students
                                            select student).ToListAsync();
            return View(students);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student p)
        {
            if (ModelState.IsValid)
            {
                context.Students.Add(p);
                await context.SaveChangesAsync();

                ViewData["Message"] = $"{p.Name} was added!";
                return View();
            }

            //Show web page with errors
            return View(p);
        }

        public async Task<IActionResult> Edit(int id)
        {
            //get the product by id
            Student? studentToEdit = await context.Students.FindAsync(id);

            //show it on web page
            if (studentToEdit == null)
            {
                return NotFound();
            }

            return View(studentToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Student p)
        {
            if (ModelState.IsValid)
            {
                context.Students.Update(p);
                await context.SaveChangesAsync();

                TempData["Message"] = $"{p.Name} was Updated!";
                return RedirectToAction("Index");
            }
            //return view with errors
            return View(p);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Student? studentToDelete = await context.Students.FindAsync(id);

            if (studentToDelete == null)
            {
                return NotFound();
            }
            return View(studentToDelete);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirm(int id)
        {
            Student studentToDelete = await context.Students.FindAsync(id);

            if (studentToDelete != null)
            {
                context.Students.Remove(studentToDelete);
                await context.SaveChangesAsync();
                TempData["Message"] = studentToDelete.Name + " was deleted successfully";
                return RedirectToAction("index");
            }
            TempData["Message"] = "This student was already deleted";
            return RedirectToAction("Index");
        }
    }
}
