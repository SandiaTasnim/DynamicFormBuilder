using DynamicFormBuilder.Models;
using DynamicFormBuilder.ViewModels;
using System.Collections.Generic;

namespace DynamicFormBuilder.Services.Interfaces
{
    public interface ICustomerService
    {
        IEnumerable<CustomerViewModel> GetAllCustomers();
        IEnumerable<CustomerViewModel> GetAllSearchCustomer(string name, string phone);

        Customer GetCustomerById(int id);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int id);

        List<Division> GetAllDivisions();
        List<District> GetAllDistricts();
        //List<District> GetAllDistricts(int divId);
        List<District> GetDistrictsByDivision(int? divisionId);
        // ICustomerService
        string GetDivisionName(int divisionId);
        string GetDistrictName(int districtId);
        //IQueryable<CustomerViewModel> GetAllCustomersQueryable();
        bool isExistNID(string NID);
        bool isExistPhone(string Phone);
    }
}
