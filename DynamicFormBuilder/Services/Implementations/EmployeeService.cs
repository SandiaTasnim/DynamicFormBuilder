using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DynamicFormBuilder.Models;
using DynamicFormBuilder.Services.Interfaces;
using ISMS.Web.Areas.Admin.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DynamicFormBuilder.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

       

        public EmployeeService( ApplicationDbContext context

        )
        {
            _context = context;


        }
        public EmployeeModel GetById(string id)
        {
            // Fetch employee by integer Id
            var employee = _context.Employees.FirstOrDefault(x => x.Id == id);

            if (employee == null)
                return null;

            // Map Employee entity to EmployeeModel
            return new EmployeeModel
            {
                Id = employee.Id,                  // int type
                EmployeeId = employee.EmployeeId,
                FullName = employee.FullName,
                Email = employee.Email,
                DOB = employee.DOB,
                Designation = employee.Designation,
                IsActive = employee.IsActive
                // StatusList is UI-only, populate in controller if needed
            };
        }

        public EmployeeModel GetEmployeeById(string id)
        {
            return _context.Employees.Find(id);
        }

        public EmployeeModel GetEmployeeDataByEmployeeId(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
                return null;

            var employee = _context.Employees.FirstOrDefault(x => x.EmployeeId == employeeId);
            if (employee == null)
                return null;

            // Map Employee to EmployeeModel
            return employee;


        }

        public IEnumerable<EmployeeModel> GetAll()
        {

            return _context.Employees.ToList();
        }
        

        //public EmployeeModel GetById(int id)
        //{
        //    return _context.Employees.FirstOrDefault(e => e.Id == id); 
        //}

        public void addEmployee(EmployeeModel employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges(); // Synchronous
        }

        //public void Update(EmployeeModel employee)
        //{
        //    var existing = _context.Employees.FirstOrDefault(e => e.Id == employee.Id);
        //    if (existing == null)
        //        throw new Exception("Employee not found");

        //    // Update fields
        //    existing.FullName = employee.FullName;
        //    existing.Email = employee.Email;
        //    existing.Designation = employee.Designation;
        //    existing.IsActive = employee.IsActive;
        //    existing.DOB = employee.DOB;

        //    _context.SaveChanges(); // Synchronous
        //}
        public EmployeeModel Update(EmployeeModel employee)
        {
            var existing = _context.Employees.FirstOrDefault(e => e.Id == employee.Id);
            if (existing == null)
                throw new Exception("Employee not found");

            existing.EmployeeId=employee.EmployeeId;
            existing.FullName = employee.FullName;
            existing.Email = employee.Email;
            existing.Designation = employee.Designation;
            existing.IsActive = employee.IsActive;
            existing.DOB = employee.DOB;

            _context.SaveChanges();
            return existing;

            //return employee; // return the updated model
        }
        public EmployeeModel UpdateStatus(EmployeeModel employee)
        {
            var existing = _context.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
            if (existing == null)
                throw new Exception("Employee not found");

            existing.FullName = existing.FullName;
            existing.Email = existing.Email;
            existing.Designation = existing.Designation;
            existing.IsActive = employee.IsActive;
            existing.DOB = existing.DOB;
            _context.Employees.Update(existing);
            _context.SaveChanges();

            return employee; 
        }

        public void delete(string id)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
                throw new Exception("Employee not found");

            _context.Employees.Remove(employee);
            _context.SaveChanges(); 
        }

        public IEnumerable<EmployeeChangeHistoriesModel> GetEmployeeChangeHistoryRecord(string mainTableId)
        {
            try
            {
                return _context.EmployeeChangeHistories
                               .Where(x => x.EmployeeId == mainTableId)
                               .OrderByDescending(x => x.ChangedAt)
                               .Select(x => new EmployeeChangeHistoriesModel
                               {
                                   EmployeeId = x.EmployeeId,
                                   PreviousData = x.PreviousData,
                                   UpdatedData = x.UpdatedData,
                                                                     
                                   ChangedAt = x.ChangedAt
                               })
                               .ToList();
            }
            catch
            {
                return Enumerable.Empty<EmployeeChangeHistoriesModel>();
            }
        }
        


            
      
        public bool IsEmployeeIdExist(string employeeId)
        {
            return _context.Employees.Any(e => e.EmployeeId == employeeId);
            
        }
        // In your EmployeeService
     
    }
}
