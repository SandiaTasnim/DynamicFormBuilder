using DynamicFormBuilder.Models;

namespace DynamicFormBuilder.Services.Interfaces
{
    public interface IEmployeeService
    {

        IEnumerable<EmployeeModel> GetAll();
        EmployeeModel GetById(string id);
        EmployeeModel GetEmployeeById(string id);
        void addEmployee(EmployeeModel employee);
        EmployeeModel Update(EmployeeModel employee);
        
        void delete(string id); 
        bool IsEmployeeIdExist(string employeeId);
      
        EmployeeModel GetEmployeeDataByEmployeeId(string employeeId);
        IEnumerable<EmployeeChangeHistoriesModel> GetEmployeeChangeHistoryRecord(string mainTableId);


    }
}


