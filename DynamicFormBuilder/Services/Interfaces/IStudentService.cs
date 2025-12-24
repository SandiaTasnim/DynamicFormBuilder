using DynamicFormBuilder.Models;

namespace DynamicFormBuilder.Services.Interfaces
{
    public interface IStudentService
    {
        List<StudentModel> GetAllStudents();
        void AddStudent(StudentModel model);

        StudentModel GetStudentById (int id);
        //void RemoveStudent(StudentModel model);

    }
}
