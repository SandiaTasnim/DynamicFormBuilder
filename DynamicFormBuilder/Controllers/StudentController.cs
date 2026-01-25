using DynamicFormBuilder.Models;
using DynamicFormBuilder.Services.Implementations;
using DynamicFormBuilder.Services.Interfaces;
using DynamicFormBuilder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList.Extensions;

namespace DynamicFormBuilder.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private ApplicationDbContext _context;

        public StudentController(IStudentService studentService, ApplicationDbContext context)
        {
            _studentService = studentService;
            _context = context;
        }

        public IActionResult Index(int? page,int pageSize=10)
        {
            var studentViewModels = _studentService.GetAllStudents();

            ViewBag.Pagesize = pageSize;
            return View(studentViewModels.ToPagedList(page ?? 1, pageSize));
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Student model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            // DO NOT SET StudentId
            _context.Students.Add(model);
            _context.SaveChanges(); // StudentId is generated here

            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            //var student = _studentService.GetStudentDetailsById(id);
            //if (student == null)
            //{
            //    return NotFound();
            //}
            //return View(student);
            var student = _studentService.GetStudentById(id);
            if (student == null)
            {
                // For AJAX requests, return error message in partial view
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    ViewBag.Message = "Employee not found.";
                    return PartialView("Details", new List<StudentViewModel>());
                }
                return NotFound();
            }

            var details = _studentService.GetStudentDetailsById(student.StudentId);


            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Details", details);
            }

            // For direct browser access, return the full view with layout
            return View(details);

        }

        public IActionResult Delete(int id)
        {
        

            var student = _studentService.GetStudentById(id);

            if (student == null)
            {
                ViewBag.Message = "Student not found.";
                // For AJAX requests (modal), return partial view without layout
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("Delete", new Student());
                }
                return NotFound();
            }

            // Check if it's an AJAX request - return ONLY the partial view (no layout/nav)
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Delete", student);
            }

            // For direct browser access, return the full view with layout
            return View("Delete", student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
           

            var employee = _studentService.GetStudentById(id);

            if (employee == null)
            {
                // For AJAX requests
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Employee not found." });
                }
                return NotFound();
            }

            _studentService.StudentDelete(id);

            // Return JSON for AJAX request (modal)
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Student deleted successfully" });
            }

            TempData["success"] = "Student deleted successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
          

            var student = _studentService.GetStudentById(id);
            if (student == null)
            {
                ViewBag.Message = "Student not found.";
                // For AJAX requests (modal), return partial view without layout
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("Edit", new Student());
                }
                return NotFound();
            }

            // Check if it's an AJAX request - return ONLY the partial view (no layout/nav)
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Edit", student);
            }

            // For direct browser access, return the full view with layout
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student model)
        {
            if (!ModelState.IsValid)
            {
                // Return partial view with validation errors for AJAX (modal)
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("Edit", model);
                }
                return View(model);
            }

            try
            {
                _studentService.Update(model);

                // Return JSON for AJAX request (modal)
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Student updated successfully" });
                }

                TempData["success"] = "Student updated successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = ex.Message });
                }

                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }


    }






