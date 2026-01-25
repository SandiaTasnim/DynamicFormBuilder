using DocumentFormat.OpenXml.Bibliography;
using DynamicFormBuilder.Models;
using DynamicFormBuilder.Services.Interfaces;
using DynamicFormBuilder.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DynamicFormBuilder.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _dbContext;
        public StudentService(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        //public List<StudentModel> GetAllStudents()
        //{
        //    var students = _dbContext.EmployeeModels.ToList();
        //    return students;
        //}

        public List<StudentViewModel> GetAllStudents()
        {
            return _dbContext.Students
                .Select(s => new StudentViewModel
                {
                    StudentId = s.StudentId,
                    StudentName = s.StudentName,
                    Age = s.Age,
                    PhoneNumber = s.PhoneNumber,
                    Email = s.Email,
                    Address = s.Address,
                    Department = s.Department
                })
                .ToList();
        }
        public Student GetStudentById(int id)
        {
            return _dbContext.Students.Find(id);//SELECT * FROM STUDENT WHERE ID =@id;
        }

        public void AddStudent(Student student)
        {
            _dbContext.Add(student);
            _dbContext.SaveChanges();
        }

        public bool IsStudentIdExist(int studentId)
        {
            return _dbContext.Students.Any(e => e.StudentId == studentId);

        }
        public Student Update(Student student)
        {
            var existing = _dbContext.Students.Find(student.StudentId);
            if (existing == null)
                throw new Exception("Student not found");

            // Update properties (don't update StudentId - it's the primary key)
            existing.StudentName = student.StudentName;
            existing.Email = student.Email;
            existing.Department = student.Department;
            existing.PhoneNumber = student.PhoneNumber;
            existing.Address = student.Address;

            _dbContext.SaveChanges();
            return existing;
        }

        public StudentViewModel GetStudentDetailsById(int id)
        {

            var student = _dbContext.Students.Where(s => s.StudentId == id).Select(x => new StudentViewModel
            {
                StudentId = x.StudentId,
                StudentName = x.StudentName,
                Email = x.Email,
                Age=x.Age,
                Department = x.Department,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber

            }).FirstOrDefault();

            return student;
        }

        public void StudentDelete(int id)
        {
            var student = _dbContext.Students.FirstOrDefault(e => e.StudentId == id);
            if (student == null)
                throw new Exception("Employee not found");

            _dbContext.Students.Remove(student);
            _dbContext.SaveChanges();
        }
    }
}
