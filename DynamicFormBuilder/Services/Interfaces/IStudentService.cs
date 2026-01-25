using DynamicFormBuilder.Models;
using DynamicFormBuilder.ViewModels;

namespace DynamicFormBuilder.Services.Interfaces
{
    public interface IStudentService
    {
        List<StudentViewModel> GetAllStudents();
        StudentViewModel GetStudentDetailsById(int id);
        void AddStudent(Student student);

        Student GetStudentById (int id);
        //void RemoveStudent(StudentModel model);
        bool IsStudentIdExist(int studentId);

        void StudentDelete(int id);
        Student Update(Student student);
       
    }
}
