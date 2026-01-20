using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using DynamicFormBuilder.Services;
using DynamicFormBuilder.Services.Interfaces;
using DynamicFormBuilder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Numerics;
using X.PagedList;
using X.PagedList.Extensions;

[Authorize]
public class CustomerController : Controller
{
    
    private readonly ICustomerService _customerService;
    private readonly ApplicationDbContext _db;

    public object GlobalValidationMessage { get; private set; }

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    // GET: Customers/Index

    //[HttpGet("Index")] // <-- Only one route per action

    //public IActionResult Index(string name, string phone, string division)
    //{

    //    var customerData = _customerService.GetAllSearchCustomer(name, phone);
    //    // Get all customers
    //    var customers = _customerService.GetAllCustomers();


    //    var divisions = _customerService.GetAllDivisions() ?? new List<Division>();
    //    ViewBag.DivisionList = divisions.Select(d => new SelectListItem
    //    {
    //        Value = d.DivisionID.ToString(),
    //        Text = d.DivisionName
    //    }).ToList();

    //    // Keep search values
    //    ViewBag.SearchName = name;
    //    ViewBag.SearchPhone = phone;



    //    return View(customerData);
    //}
    //[HttpGet("Index")] // <-- Only one route per action
    //public IActionResult Index(string name, string phone, int page = 1)
    //{
    //    int pageSize = 10;  // ADD THIS - records per page

    //    // Get all customers
    //    var customers = _customerService.GetAllCustomers()
    //             ?? Enumerable.Empty<CustomerViewModel>();

    //    // APPLY SEARCH FILTERS FIRST (before pagination)
    //    if (!string.IsNullOrWhiteSpace(name))
    //        customers = customers.Where(c =>
    //            (c.FirstName != null && c.FirstName.Contains(name)) ||
    //            (c.LastName != null && c.LastName.Contains(name)));

    //    if (!string.IsNullOrWhiteSpace(phone))
    //        customers = customers.Where(c => c.Phone != null && c.Phone.Contains(phone));

    //    // CALCULATE TOTALS AFTER FILTERING
    //    int totalRecords = customers.Count();
    //    int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

    //    // APPLY PAGINATION
    //    var customersPaged = customers.Skip((page - 1) * pageSize).Take(pageSize).ToList();



    //    // Keep search values
    //    ViewBag.SearchName = name;
    //    ViewBag.SearchPhone = phone;
    //    ViewBag.CurrentPage = page;        
    //    ViewBag.TotalPages = totalPages;  
    //    ViewBag.TotalRecords = totalRecords; 
    //    return View(customersPaged);  // CHANGED from customerData to customersPaged
    //}


    // using X.PagedList;
    [HttpGet("Index")]
    public IActionResult Index(string name, string phone, int? page, int pageSize = 10)
    {
        if (pageSize <= 0) pageSize = 10;

    
        ViewBag.SearchName = name;
        ViewBag.SearchPhone = phone;

        // 1️⃣ Get entity data (NULL SAFE)
        var customers = _customerService.GetAllSearchCustomer(name, phone);
        // 2️⃣ APPLY SEARCH (optional but recommended)
        
        var vm = customers.Select(c => new CustomerViewModel
        {
            CustomerID = c.CustomerID,
            FirstName = c.FirstName,
            LastName = c.LastName,
            FullName = c.FullName,
            Phone = c.Phone,
            Email = c.Email,
            Profession = c.Profession,
            Balance = c.Balance,
            DivisionName = c.DivisionName,
            DistrictName = c.DistrictName,
            NID = c.NID
        });

        

        return View(vm.ToPagedList(page ?? 1, pageSize));
    }





    [HttpGet("Create")] 

    public IActionResult Create()
    {
        ViewBag.DivisionsList = _customerService.GetAllDivisions()
            .Select(d => new SelectListItem { Value = d.DivisionID.ToString(), Text = d.DivisionName })
            .ToList();

        ViewBag.DistrictsList = new List<SelectListItem>(); 
        return View();
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Customer customer)
    {
        if (_customerService.isExistNID(customer.NID))
        {
            ModelState.AddModelError(string.Empty, "NID  already exists!");

            // Convert to SelectListItem format
            ViewBag.DivisionsList = _customerService.GetAllDivisions()
                .Select(d => new SelectListItem { Value = d.DivisionID.ToString(), Text = d.DivisionName })
                .ToList();

            ViewBag.DistrictsList = _customerService.GetAllDistricts()
                .Select(d => new SelectListItem { Value = d.DistrictID.ToString(), Text = d.DistrictName })
                .ToList();

            return View(customer);
        }
        if (_customerService.isExistPhone(customer.Phone))
        {
            ModelState.AddModelError(string.Empty, "Phone already exists!");

            // Convert to SelectListItem format
            ViewBag.DivisionsList = _customerService.GetAllDivisions()
                .Select(d => new SelectListItem { Value = d.DivisionID.ToString(), Text = d.DivisionName })
                .ToList();

            ViewBag.DistrictsList = _customerService.GetAllDistricts()
                .Select(d => new SelectListItem { Value = d.DistrictID.ToString(), Text = d.DistrictName })
                .ToList();

            return View(customer);
        }

        if (!ModelState.IsValid)
        {
            
            ViewBag.DivisionsList = _customerService.GetAllDivisions()
                .Select(d => new SelectListItem { Value = d.DivisionID.ToString(), Text = d.DivisionName })
                .ToList();

            ViewBag.DistrictsList = _customerService.GetAllDistricts()
                .Select(d => new SelectListItem { Value = d.DistrictID.ToString(), Text = d.DistrictName })
                .ToList();

            return View(customer);
        }

        try
        {
            _customerService.AddCustomer(customer);
            return RedirectToAction("Index");
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "An error occurred while creating the customer.");

            
            ViewBag.DivisionsList = _customerService.GetAllDivisions()
                .Select(d => new SelectListItem { Value = d.DivisionID.ToString(), Text = d.DivisionName })
                .ToList();

            ViewBag.DistrictsList = _customerService.GetAllDistricts()
                .Select(d => new SelectListItem { Value = d.DistrictID.ToString(), Text = d.DistrictName })
                .ToList();

            return View(customer);
        }
    }



    //public IActionResult Details(int id)
    //{
    //    var customer = _customerService.GetCustomerById(id);
    //    if (customer == null) return NotFound();


    //    ViewBag.DivisionName = customer.DivisionID == null
    //        ? "N/A"
    //        : _customerService.GetDivisionName(customer.DivisionID.Value);

    //    ViewBag.DistrictName = customer.DistrictID == null
    //        ? "N/A"
    //        : _customerService.GetDistrictName(customer.DistrictID.Value);

    //    return View(customer);
    //}


    public IActionResult Details(int id)
    {
        var customer = _customerService.GetCustomerDetailsById(id);

        if (customer == null)
            return NotFound();
        return View("Details", customer);
    }


    [HttpGet("Edit")]
    public IActionResult Edit(int id)
    {

        var customer = _customerService.GetCustomerById(id);
        if (customer == null) return NotFound();

        
        // Populate division select list (selected = customer's division)

        ViewBag.DivisionsList = new SelectList(
            _customerService.GetAllDivisions(),
            "DivisionID",
            "DivisionName",
            customer.DivisionID);

        // Populate district select list for the selected division (selected = customer's district)
        ViewBag.DistrictsList = new SelectList(
            _customerService.GetDistrictsByDivision(customer.DivisionID),
            "DistrictID",
            "DistrictName",
            customer.DistrictID);

        
        return View("Edit", customer);
    }



    public IActionResult UploadExcel()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UploadExcel(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            TempData["Message"] = "No file selected!";
            return RedirectToAction("Index");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (extension != ".xlsx")
        {
            TempData["Message"] = "Please upload a valid Excel file (.xlsx)";
            return RedirectToAction("Index");
        }

        int updated = 0;
        int notFound = 0;

        try
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using var package = new ExcelPackage(stream);
            var sheet = package.Workbook.Worksheets.FirstOrDefault();

            if (sheet?.Dimension == null)
            {
                TempData["Message"] = "Excel file is empty or invalid!";
                return RedirectToAction("Index");
            }

            var allCustomers = _customerService.GetAllCustomers()?.ToList();
            if (allCustomers == null || !allCustomers.Any())
            {
                TempData["Message"] = "No customers in database!";
                return RedirectToAction("Index");
            }

            for (int row = 2; row <= sheet.Dimension.Rows; row++)
            {
                string nameFromExcel = sheet.Cells[row, 1].GetValue<string>()?.Trim();
                string professionFromExcel = sheet.Cells[row, 2].GetValue<string>()?.Trim();

                if (string.IsNullOrWhiteSpace(nameFromExcel))
                    continue;


                var normalizedExcelName = nameFromExcel.Replace("  ", " ").ToLower();

                var customer = allCustomers.FirstOrDefault(p =>
                    !string.IsNullOrWhiteSpace(p.FullName) &&
                    p.FullName.Replace("  ", " ").ToLower() == normalizedExcelName
                );

                if (customer != null)
                {
                    // Get fresh instance from DB and update
                    var dbCustomer = _customerService.GetCustomerById(customer.CustomerID);
                    if (dbCustomer != null)
                    {
                        dbCustomer.Profession = professionFromExcel;
                        _customerService.GetUpdateCustomer(dbCustomer);
                        updated++;
                    }
                }
                else
                {
                    notFound++;
                }
            }

            TempData["Message"] = $"✓ {updated} customers updated. {notFound} names not found.";
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"Error: {ex.Message}";
        }

        return RedirectToAction("Index");
    }

    [HttpPost("Edit")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Customer model)
    {
        if (_customerService.isExistNID(model.NID))
        {
            ModelState.AddModelError(string.Empty, "NID already exists!");
            

        }
        if (_customerService.isExistPhone(model.Phone))
        {
            ModelState.AddModelError(string.Empty, "Phone already exists!");
            

        }
        if (!ModelState.IsValid)
        {

            ViewBag.DivisionsList = new SelectList(
                _customerService.GetAllDivisions(),
                "DivisionID",
                "DivisionName",
                model.DivisionID);

            ViewBag.DistrictsList = new SelectList(
                _customerService.GetDistrictsByDivision(model.DivisionID),
                "DistrictID",
                "DistrictName",
                model.DistrictID);


            
        }
        _customerService.UpdateCustomer(model);


        return RedirectToAction("Index");
    }



    [HttpGet("Delete")]
    public IActionResult Delete(int id)
    {
        var customer = _customerService.GetCustomerById(id);
        if (customer == null) return NotFound();

        // Return the view named "Delete" and pass the customer model
        return View("Delete", customer);
    }

    // POST: /Customer/Delete


    [HttpPost("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(Customer model)
    {
        var customer = _customerService.GetCustomerById(model.CustomerID);
        if (customer == null) return NotFound();

        // Call service to delete
        _customerService.DeleteCustomer(customer.CustomerID);

        // Redirect to Index after deletion
        return RedirectToAction("Index");
    }







    [HttpPost]
    public IActionResult UpdateBalance(int id, decimal newBalance)
    {
        var customer = _customerService.GetCustomerById(id);
        if (customer == null)
        {
            return NotFound();
        }

        customer.Balance = newBalance;
        _customerService.UpdateCustomer(customer);

        return RedirectToAction("Details", new { id });
    }


    // GET: Customer/GetDistricts/1
    [HttpGet]
    public JsonResult GetDistricts(int divisionId)
    {
        var districts = _customerService.GetAllDistricts()
                        .Where(d => d.DivisionID == divisionId)
                        .Select(d => new { d.DistrictID, d.DistrictName })
                        .ToList();

        return Json(districts);
    }


    public IActionResult Excel_Download()
    {

        //================================FOR EXCEL EXPORT REPORT================================
        var sFileName = "Customer Details" + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";

        using (var workbook = new XLWorkbook())
        {
            //===FOR Login History EXCEL EXPORT REPORT===

            //var data = _customerService.GetAllSearchCustomer(name, phone);
            var data = _customerService.GetAllCustomers();



            var ws = workbook.Worksheets.Add("Report");
            ws.RowHeight = 20;

            var range = ws.Range("A1:K1");
            range.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            range.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            range.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            range.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            range.Style.Border.TopBorderColor = XLColor.Black;
            range.Style.Border.LeftBorderColor = XLColor.Black;
            range.Style.Border.RightBorderColor = XLColor.Black;
            range.Style.Border.BottomBorderColor = XLColor.Black;


            ws.Row(1).Style.Fill.PatternType = XLFillPatternValues.Solid;
            ws.Row(1).Style.Fill.BackgroundColor = XLColor.FromArgb(200, 200, 198);
            ws.Row(1).Height = 20;
            ws.Row(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Row(1).Style.Font.Bold = true;
            ws.Row(1).Style.Font.FontSize = 12;


            if (data != null)
            {
                var headerColumn = 1;
                ws.Cell(1, headerColumn++).Value = "Full Name";
                ws.Cell(1, headerColumn++).Value = "Profession";
                ws.Cell(1, headerColumn++).Value = "Phone";
                ws.Cell(1, headerColumn++).Value = "District";
                ws.Cell(1, headerColumn++).Value = "Division";
                ws.Cell(1, headerColumn++).Value = "Balance";
                ws.Cell(1, headerColumn++).Value = "Email";
                ws.Cell(1, headerColumn++).Value = "NID";

            }

            if (data.Any())
            {
                int rowId = 2;

                foreach (var singleData in data)
                {
                    var DataColumn = 1;
                    ws.Cell(rowId, DataColumn++).Value = singleData.FullName;
                    ws.Cell(rowId, DataColumn++).Value = singleData.Profession;
                    ws.Cell(rowId, DataColumn++).Value = singleData.Phone;
                    ws.Cell(rowId, DataColumn++).Value = singleData.DistrictName;
                    ws.Cell(rowId, DataColumn++).Value = singleData.DivisionName;
                    ws.Cell(rowId, DataColumn++).Value = singleData.Balance;
                    ws.Cell(rowId, DataColumn++).Value = singleData.Email;
                    ws.Cell(rowId, DataColumn++).Value = singleData.NID;

                    rowId++;
                }
            }


            for (int i = 1; i <= 11; i++)
            {
                ws.Column(i).AdjustToContents();
                ws.Column(i).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Column(i).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
            }
        }

               
    }

    [HttpPost("ReadExcelFileAsync")]
    public async Task<IActionResult> ReadExcelFileAsync(IFormFile file, string name,string phone)

    {
        if (file == null || file.Length == 0)
            return Content("File Not Selected");

        string fileExtension = Path.GetExtension(file.FileName);
        if (fileExtension != ".xls" && fileExtension != ".xlsx")
            return Content("File Not Selected");

        var rootFolder = @"D:\Files";
        var fileName = file.FileName;
        var filePath = Path.Combine(rootFolder, fileName);
        var fileLocation = new FileInfo(filePath);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        if (file.Length <= 0)
            return BadRequest("File not found.");

        using (ExcelPackage package = new ExcelPackage(fileLocation))
        {
            ExcelWorksheet workSheet = package.Workbook.Worksheets["Table1"];
            int totalRows = workSheet.Dimension.Rows;
            var DataList = new List<Customer>();


            for (int i = 2; i <= totalRows; i++)
            {
                DataList.Add(new Customer
                {
                    FirstName = workSheet.Cells[i, 1].Text,
                    LastName = workSheet.Cells[i, 2].Text,
                    Phone = workSheet.Cells[i, 3].Text,
                    Email = workSheet.Cells[i, 4].Text,
                    Profession = workSheet.Cells[i, 5].Text,

                    // Balance (string → decimal)
                    Balance = decimal.TryParse(workSheet.Cells[i, 6].Text, out decimal balance)
                        ? balance
                        : 0,

                    NID = workSheet.Cells[i, 7].Text,
                    
                });


            }


            _db.Customers.AddRange(DataList);
            _db.SaveChanges();
        }
        return Ok();
    }

    



    //public IActionResult Excel_Download(string name, string phone, string division)
    //{
    //    var sFileName = "MemberWise_Details_Payment_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
    //    var data = _customerService.GetAllSearchCustomer(name, phone, division);

    //    using (var workbook = new XLWorkbook())
    //    {
    //        var ws = workbook.Worksheets.Add("Report");

    //        // Header
    //        ws.Cell(1, 1).Value = "Full Name";
    //        ws.Cell(1, 2).Value = "DOB";
    //        ws.Cell(1, 3).Value = "Profession";
    //        ws.Cell(1, 4).Value = "Phone";
    //        ws.Cell(1, 5).Value = "District";
    //        ws.Cell(1, 6).Value = "Division";
    //        ws.Cell(1, 7).Value = "Balance";
    //        ws.Cell(1, 8).Value = "Email";
    //        ws.Cell(1, 9).Value = "NID";

    //        // Make header bold
    //        ws.Row(1).Style.Font.Bold = true;

    //        int rowId = 2;
    //        foreach (var singleData in data)
    //        {
    //            ws.Cell(rowId, 1).Value = singleData.FullName;
    //            ws.Cell(rowId, 2).Value = singleData.DOB?.ToString("yyyy-MM-dd");
    //            ws.Cell(rowId, 3).Value = singleData.Profession;
    //            ws.Cell(rowId, 4).Value = singleData.Phone;
    //            ws.Cell(rowId, 5).Value = singleData.DistrictName;
    //            ws.Cell(rowId, 6).Value = singleData.DivisionName;
    //            ws.Cell(rowId, 7).Value = singleData.Balance;
    //            ws.Cell(rowId, 8).Value = singleData.Email;
    //            ws.Cell(rowId, 9).Value = singleData.NID;
    //            rowId++;
    //        }

    //        ws.Columns().AdjustToContents();

    //        using (var stream = new MemoryStream())
    //        {
    //            workbook.SaveAs(stream);
    //            var content = stream.ToArray();
    //            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
    //        }
    //    }

    //}

}





































































































































































































































































































































































































































































































































































































































































