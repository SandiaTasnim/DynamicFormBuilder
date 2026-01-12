using DynamicFormBuilder.Models;
using DynamicFormBuilder.Services.Interfaces;

namespace DynamicFormBuilder.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _dbContext;
        public StudentService(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public void AddStudent(StudentModel model)
        {
            _dbContext.Add(model);
            _dbContext.SaveChanges();
        }

        public List<StudentModel> GetAllStudents()
        {
            var students = _dbContext.EmployeeModels.ToList();
            return students;
        }

        public StudentModel GetStudentById(int id)
        {
            return _dbContext.EmployeeModels.Find(id);//SELECT * FROM STUDENT WHERE ID =@id;
        }
    }
}
