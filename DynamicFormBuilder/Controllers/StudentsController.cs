using Microsoft.AspNetCore.Mvc;
using DynamicFormBuilder.Models;
using System.Collections.Generic;
using DynamicFormBuilder.Services.Interfaces;

namespace DynamicFormBuilder.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: /Customer/Index
        public IActionResult Index()
        {
            var customers = _customerService.GetAllCustomers();
            return View(customers);
        }

        // GET: /Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Customer/Create
        [HttpPost]
        public IActionResult Create(Customer model)
        {
            if (ModelState.IsValid)
            {
                _customerService.AddCustomer(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}