using DynamicFormBuilder.Models;
using DynamicFormBuilder.Services.Interfaces;
using DynamicFormBuilder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace DynamicFormBuilder.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _db;

        public CustomerService(ApplicationDbContext db)
        {
            _db = db;
        }

        // Get all customers
        public IEnumerable<CustomerViewModel> GetAllCustomers()
        {
            var result = from c in _db.Customers
                         join d in _db.Divisions on c.DivisionID equals d.DivisionID into divGroup
                         from d in divGroup.DefaultIfEmpty()  // Left join to allow null
                         join t in _db.Districts on c.DistrictID equals t.DistrictID into distGroup
                         from t in distGroup.DefaultIfEmpty()  // Left join to allow null
                         select new CustomerViewModel
                         {
                             CustomerID = c.CustomerID,
                             FullName = c.FirstName + " " + c.LastName,
                             Phone = c.Phone,
                             Email = c.Email,
                             Profession = c.Profession,
                             DOB = c.DOB,
                             Balance = c.Balance,
                             NID = c.NID,
                             DivisionID = c.DivisionID,
                             DistrictID = c.DistrictID,
                             DivisionName = d.DivisionName,
                             DistrictName = t.DistrictName
                         };


            return result.ToList();
        }
        public IEnumerable<CustomerViewModel> GetAllSearchCustomer(string name, string phone)
        {
            var customers = GetAllCustomers();

            // Search by name
            if (!string.IsNullOrWhiteSpace(name))
            {
                customers = customers
                    .Where(c => c.FullName != null && c.FullName.Contains(name))
                    .ToList();
            }

            // Filter by phone
            if (!string.IsNullOrWhiteSpace(phone))
            {
                customers = customers
                    .Where(c => c.Phone != null && c.Phone.Contains(phone))
                    .ToList();
            }

            


            return customers.ToList();
        }

        public bool isExistNID(string NID)
        {
            return _db.Customers.Any(m => m.NID == NID );
        }
        public bool isExistPhone( string Phone)
        {
            return _db.Customers.Any(m =>  m.Phone == Phone);
        }
       
        // Get customer by ID
        
        public Customer GetCustomerById(int id)
        {
            return _db.Customers.FirstOrDefault(c => c.CustomerID == id);
        }

        // Add a new customer
        public void AddCustomer(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            if (customer.Balance == default) customer.Balance = 0m;

            _db.Customers.Add(customer);
            _db.SaveChanges();
        
        }
        //Delete customer
        public void DeleteCustomer(int id)
        {
            // Find the customer by ID
            var customer = _db.Customers.FirstOrDefault(c => c.CustomerID == id);

            if (customer == null)
            {
                throw new Exception("Customer not found");
            }

            // Remove the customer
            _db.Customers.Remove(customer);

            // Save changes to database
            _db.SaveChanges();
        }


        // Update existing customer
        public void UpdateCustomer(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));

            var existing = _db.Customers.FirstOrDefault(c => c.CustomerID == customer.CustomerID);
            if (existing == null) throw new Exception("Customer not found");

            existing.FirstName = customer.FirstName;
            existing.LastName = customer.LastName;
            existing.Phone = customer.Phone;
            existing.Email = customer.Email;
            existing.NID = customer.NID;
            existing.DivisionID = customer.DivisionID;
            existing.DistrictID = customer.DistrictID;
            existing.DOB = customer.DOB;
            existing.Profession = customer.Profession;
            existing.Balance = customer.Balance;

            _db.SaveChanges();
        }

        public List<Division> GetAllDivisions()
        {
            return _db.Divisions.ToList();
        }
        public List<District> GetAllDistricts()
        {
            return _db.Districts.ToList();
        }
        public List<District> GetDistrictsByDivision(int? divisionId)
        {
            if (divisionId == null)
                return new List<District>();

            return _db.Districts
                      .Where(d => d.DivisionID == divisionId.Value)
                      .OrderBy(d => d.DistrictName)   // optional ordering
                      .ToList();
        }
        // CustomerService (EF Core)
        public string GetDivisionName(int divisionId)
        {
            var division = _db.Divisions.Find(divisionId);
            return division?.DivisionName ?? "N/A";
        }

        public string GetDistrictName(int districtId)
        {
            var district = _db.Districts.Find(districtId);
            return district?.DistrictName ?? "N/A";
        }


    }
}
