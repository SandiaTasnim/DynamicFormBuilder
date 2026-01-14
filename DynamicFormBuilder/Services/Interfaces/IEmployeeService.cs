using DynamicFormBuilder.Models;
using DynamicFormBuilder.ViewModels;

namespace DynamicFormBuilder.Services.Interfaces
{
    public interface IEmployeeService
    {

        IEnumerable<EmployeeModel> GetAll();
        EmployeeModel GetById(string id);
        //void UpdateStatus(string employeeId);
        EmployeeModel GetEmployeeById(string id);
        void addEmployee(EmployeeModel employee);
        EmployeeModel Update(EmployeeModel employee);
        
        void delete(string id); 
        bool IsEmployeeIdExist(string employeeId);
        void UpdateWithHistory(string employeeId, bool newStatus);


        EmployeeModel GetEmployeeDataByEmployeeId(string employeeId);
        IEnumerable<EmployeeChangeHistoriesViewModel> GetEmployeeChangeHistoryRecord(string employeeId);
        //bool CheckPrevStatus(EmployeeModel data);
    }
}


