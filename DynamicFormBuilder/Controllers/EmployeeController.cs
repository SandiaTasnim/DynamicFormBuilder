using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes; 
using DynamicFormBuilder.Services.Interfaces;
using DynamicFormBuilder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Utilities;
using OfficeOpenXml.Style;
using OfficeOpenXml.Style.XmlAccess;
using System.ComponentModel;
using System.Data;
using System.Formats.Asn1;
using System.Globalization;
using System.IO.Compression;
using System.Security;
using System.Transactions;
using X.PagedList;
using X.PagedList.Extensions;

namespace DynamicFormBuilder.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {

        private IEmployeeService _employeeService;
        private ApplicationDbContext _context;


        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;


        }
        public IActionResult Index(int? page, int pageSize = 10)
        {
            

            var employees = _employeeService.GetAll();
            var employeeViewModels = employees.Select(e => new EmployeeViewModel
            {
                Id = e.Id,
                EmployeeId = e.EmployeeId,
                FullName = e.FullName,
                Email = e.Email,
                Designation = e.Designation,
                IsActive = e.IsActive
            });
            ViewBag.PageSize = pageSize;
            return View(employeeViewModels.ToPagedList(page??1, pageSize));


        }


        public IActionResult Create()
        {
            var model = new EmployeeModel
            {
                StatusList = new SelectList(
                    new []
                    {
                new SelectListItem { Text = "Active", Value = "true" },
                new SelectListItem { Text = "Inactive", Value = "false" }
                    },
                    "Value",
                    "Text"
                )
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
                model.Id = Guid.NewGuid().ToString();



            if (_employeeService.IsEmployeeIdExist(model.EmployeeId))
            {
                ModelState.AddModelError("EmployeeId", "This Employee ID is already taken!");
            }


            if (!ModelState.IsValid)
            {
                model.StatusList = new SelectList(
                    new List<SelectListItem>
                    {
                new SelectListItem { Text = "Active", Value = "true", Selected = model.IsActive },
                new SelectListItem { Text = "Inactive", Value = "false", Selected = !model.IsActive }
                    },
                    "Value",
                    "Text"
                );

                return View(model);
            }
            _employeeService.addEmployee(model); 

            TempData["success"] = "Employee created successfully";
            return RedirectToAction("Index");

        }



        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// CREATE - POST
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(EmployeeModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    _employeeService.Create(model);  // synchronous call
        //    TempData["success"] = "Employee created successfully";
        //    return RedirectToAction("Index");
        //}

        // EDIT - GET
        //public IActionResult Edit(string id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //        return NotFound();

        //    var employee = _employeeService.GetById(id); // synchronous call
        //    if (employee == null)
        //        return NotFound();


        //    return View(employee);
        //}
        //[HttpGet]
        //public IActionResult Delete(string id)
        //{
        //    var employee = _employeeService.GetEmployeeById(id);
        //    if (employee == null) return NotFound();

        //    // Return the view named "Delete" and pass the customer model
        //    return View("Delete", employee);
        //}

        //// POST: /Customer/Delete


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteConfirmed(string id)
        //{
        //    var employee = _employeeService.GetEmployeeById(id);
        //    if (employee == null) return NotFound();
        //    _employeeService.delete(id);
        //    return RedirectToAction("Index");
        //}


        //public IActionResult Edit(string id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        // For AJAX requests, return error message in partial view
        //        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //        {
        //            ViewBag.Message = "Employee not found.";
        //            return PartialView("Edit", new EmployeeModel());
        //        }
        //        return NotFound();
        //    }

        //    var employee = _employeeService.GetById(id);

        //    if (employee == null)
        //    {
        //        ViewBag.Message = "Employee not found.";
        //        // For AJAX requests (modal), return partial view without layout
        //        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //        {
        //            return PartialView("Edit", new EmployeeModel());
        //        }
        //        return NotFound();
        //    }

        //    employee.StatusList = new SelectList(
        //        new List<SelectListItem>
        //        {
        //    new SelectListItem { Text = "Active", Value = "true", Selected = employee.IsActive },
        //    new SelectListItem { Text = "Inactive", Value = "false", Selected = !employee.IsActive }
        //        },
        //        "Value",
        //        "Text",
        //        employee.IsActive ? "true" : "false"
        //    );

        //    // Check if it's an AJAX request - return ONLY the partial view (no layout/nav)
        //    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //    {
        //        return PartialView("Edit", employee);
        //    }

        //    // For direct browser access, return the full view with layout
        //    return View(employee);
        //}

        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                // For AJAX requests, return error message in partial view
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    ViewBag.Message = "Employee not found.";
                    return PartialView("Delete", new EmployeeModel());
                }
                return NotFound();
            }

            var employee = _employeeService.GetEmployeeById(id);

            if (employee == null)
            {
                ViewBag.Message = "Employee not found.";
                // For AJAX requests (modal), return partial view without layout
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("Delete", new EmployeeModel());
                }
                return NotFound();
            }

            // Check if it's an AJAX request - return ONLY the partial view (no layout/nav)
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Delete", employee);
            }

            // For direct browser access, return the full view with layout
            return View("Delete", employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                // For AJAX requests
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Employee ID is required." });
                }
                return NotFound();
            }

            var employee = _employeeService.GetEmployeeById(id);

            if (employee == null)
            {
                // For AJAX requests
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Employee not found." });
                }
                return NotFound();
            }

            _employeeService.delete(id);

            // Return JSON for AJAX request (modal)
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Employee deleted successfully" });
            }

            TempData["success"] = "Employee deleted successfully";
            return RedirectToAction("Index");
        }


        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                // For AJAX requests, return error message in partial view
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    ViewBag.Message = "Employee not found.";
                    return PartialView("Edit", new EmployeeModel());
                }
                return NotFound();
            }

            var employee = _employeeService.GetById(id);

            if (employee == null)
            {
                ViewBag.Message = "Employee not found.";
                // For AJAX requests (modal), return partial view without layout
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("Edit", new EmployeeModel());
                }
                return NotFound();
            }

            employee.StatusList = new SelectList(
                new List<SelectListItem>
                {
            new SelectListItem { Text = "Active", Value = "true", Selected = employee.IsActive },
            new SelectListItem { Text = "Inactive", Value = "false", Selected = !employee.IsActive }
                },
                "Value",
                "Text",
                employee.IsActive ? "true" : "false"
            );

            // Check if it's an AJAX request - return ONLY the partial view (no layout/nav)
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Edit", employee);
            }

            // For direct browser access, return the full view with layout
            return View(employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeModel model)
        {
            if (!ModelState.IsValid)
            {
                model.StatusList = new SelectList(
                    new List<SelectListItem>
                    {
                new SelectListItem { Text = "Active", Value = "true", Selected = model.IsActive },
                new SelectListItem { Text = "Inactive", Value = "false", Selected = !model.IsActive }
                    },
                    "Value",
                    "Text",
                    model.IsActive ? "true" : "false"
                );

                // Return partial view with validation errors for AJAX (modal)
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("Edit", model);
                }

                return View(model);
            }

            _employeeService.Update(model);

            // Return JSON for AJAX request (modal)
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Employee updated successfully" });
            }

            TempData["success"] = "Employee updated successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Details(string employeeId)
        {
            //var employee = _employeeService.GetEmployeeById(id);
            if (employeeId == null)
            {
                // For AJAX requests, return error message in partial view
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    ViewBag.Message = "Employee not found.";
                    return PartialView("Details", new List<EmployeeChangeHistoriesViewModel>());
                }
                return NotFound();
            }

            var history = _employeeService.GetEmployeeChangeHistoryRecord(employeeId);

            if (history == null || !history.Any())
            {
                ViewBag.Message = "No change history found for this employee.";

                // For AJAX requests (modal), return partial view without layout
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("Details", new List<EmployeeChangeHistoriesViewModel>());
                }

                // For direct browser access, return full view with layout
                return View(new List<EmployeeChangeHistoriesViewModel>());
            }

            // Check if it's an AJAX request - return ONLY the partial view (no layout/nav)
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Details", history);
            }

            // For direct browser access, return the full view with layout
            return View(history);
        }

        #region Employee Status Update
        public ActionResult EmployeeStatusUpdate(bool searchStatus, string others, string searchEmployeeId, int page = 1, int pageSize = 30)
        {

            ViewBag.page = page;
            ViewBag.pageSize = pageSize;
            var model = new EmployeeModel()
            {
               
                IsActive = searchStatus,
                Id = searchEmployeeId,

                StatusList = new SelectList(new List<SelectListItem>
                {
                    new SelectListItem {Text = "--Select--", Value = ""},
                    new SelectListItem {Value = "ACTIVE" , Text = @"ACTIVE"},
                    new SelectListItem {Value = "INACTIVE", Text = @"INACTIVE"},
                }, "Value", "Text", 1)
            };
            return PartialView(model);
        }

        public ActionResult SearchForEmployeeStatusUpdate(string searchStatus, string others, int searchEmployeeId, int page = 1, int pageSize = 30)
        {

            ViewBag.searchEmployeeId = searchEmployeeId;
            ViewBag.status = searchStatus;
            ViewBag.page = page;
            ViewBag.pageSize = pageSize;
            ViewBag.others = others;


            return View();
        }


        [HttpGet]
        public ActionResult EmployeeStatusUpdateBulkUpload(int page = 1, int pageSize = 30)
        {
            ViewBag.page = page;
            ViewBag.pageSize = pageSize;
            return PartialView();
        }

        #region Upload Post
        [HttpPost]
        public async Task<ActionResult> EmployeeStatusUpdateBulkUpload(IFormFile fileUpload = null, int page = 1, int pageSize = 30)
        {
            var message = "File upload failed";
            var path = "";

            try
            {
                // 1️⃣ Validate file
                if (fileUpload == null || fileUpload.Length == 0)
                {
                    message = "Please select a file to upload";
                    return CreateJsonResult(page, pageSize, message);
                }

                var filename = Path.GetFileName(fileUpload.FileName);
                var fileExtension = Path.GetExtension(filename).ToLower();
                var allowedExtensions = new[] { ".csv", ".xls", ".xlsx" };

                if (!allowedExtensions.Contains(fileExtension))
                {
                    message = "Invalid file type. Only CSV and Excel files are allowed";
                    return CreateJsonResult(page, pageSize, message);
                }

                // 2️⃣ Save uploaded file
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "EmployeeStatusBulk");

                if (!CreateFolderIfNeeded(uploadFolder))
                {
                    message = "Failed to create upload directory";
                    return CreateJsonResult(page, pageSize, message);
                }

                path = Path.Combine(uploadFolder, filename);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await fileUpload.CopyToAsync(stream);
                }

                // 3️⃣ Read file into DataTable
                DataTable dt;

                if (fileExtension == ".csv")
                {
                    dt = ReadCsvFileToDataTable(path);
                }
                else // Excel file
                {
                    dt = ReadExcelFileToDataTable(path);
                }

                if (dt.Rows.Count == 0)
                {
                    message = "The uploaded file contains no data";
                    return CreateJsonResult(page, pageSize, message);
                }

                // 4️⃣ Validate data
                var empList = new List<EmployeeModel>();
                var errorList = new List<string>();
                var processedEmployeeIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var validStatuses = new[] { "Active", "Inactive" };
                int serial = 0;

                foreach (DataRow row in dt.Rows)
                {
                    serial++;

                    try
                    {
                        var employeeId = GetColumnValue(row, "Employee ID")?.Trim();

                        if (string.IsNullOrWhiteSpace(employeeId))
                        {
                            errorList.Add($"Line {serial}: Employee ID is required");
                            continue;
                        }

                        if (processedEmployeeIds.Contains(employeeId))
                        {
                            errorList.Add($"Line {serial}: Duplicate Employee ID ({employeeId}) in file");
                            continue;
                        }

                        if (!_employeeService.IsEmployeeIdExist(employeeId))
                        {
                            errorList.Add($"Line {serial}: Employee not found ({employeeId})");
                            continue;
                        }

                        var status = GetColumnValue(row, "Status")?.Trim();

                        if (string.IsNullOrWhiteSpace(status))
                        {
                            errorList.Add($"Line {serial}: Status is required for Employee ID ({employeeId})");
                            continue;
                        }

                        if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
                        {
                            errorList.Add($"Line {serial}: Invalid status '{status}'. Allowed: {string.Join(", ", validStatuses)}");
                            continue;
                        }

                        // ✅ FIX: Convert status string to boolean

                        bool isActive = status.Equals("Active", StringComparison.OrdinalIgnoreCase);
                 
                        empList.Add(new EmployeeModel
                        {
                            EmployeeId = employeeId,
                   

                            IsActive = isActive // ✅ Now properly assigned
                        });


                        processedEmployeeIds.Add(employeeId);
                    }
                    catch (Exception exRow)
                    {
                        errorList.Add($"Line {serial}: Error - {exRow.Message}");
                    }
                }

                // 5️⃣ If errors exist, return
                if (errorList.Any())
                {
                    message = "Errors found: " + string.Join("; ", errorList);
                    return CreateJsonResult(page, pageSize, message);
                }

                // 6️⃣ Update database inside a transaction
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var now = DateTime.Now;
                    int updatedCount = 0;

                    foreach (var emp in empList)
                    {
                        var data = _employeeService.GetEmployeeDataByEmployeeId(emp.EmployeeId);

                        if (data == null)
                            throw new Exception($"Employee data not found for ID: {emp.EmployeeId}");

                        // Update the status
                        // data.IsActive = emp.IsActive;
                        //data.UpdatedDate = now; // Optional: track when updated
                        //data.UpdatedBy = User.Identity.Name; // Optional: track who updated

                        // ✅ FIX: Uncommented and added error checking
                        // _employeeService.Update(data);
                        _employeeService.UpdateWithHistory(emp.EmployeeId, emp.IsActive);






                        updatedCount++;
                    }

                    scope.Complete();
                    message = $"✅ Data updated successfully! {updatedCount} employee(s) updated.";
                }
            }
            catch (Exception ex)
            {
                message = $"❌ File upload failed: {ex.Message}";
            }
            finally
            {
                // 7️⃣ Clean up uploaded file
                if (!string.IsNullOrEmpty(path) && System.IO.File.Exists(path))
                {
                    try
                    {
                        System.IO.File.Delete(path);
                    }
                    catch
                    {
                    }
                }
            }


            return CreateJsonResult(page, pageSize, message);
        }

        private DataTable ReadExcelFileToDataTable(string path)
        {
            var dt = new DataTable();

            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                var ws = package.Workbook.Worksheets.FirstOrDefault();

                if (ws == null || ws.Dimension == null)
                    return dt;

                // Add columns from first row
                for (int col = 1; col <= ws.Dimension.End.Column; col++)
                {
                    var headerValue = ws.Cells[1, col].Text?.Trim();
                    if (!string.IsNullOrWhiteSpace(headerValue))
                    {
                        dt.Columns.Add(headerValue);
                    }
                }

                // Add data rows (skip empty rows)
                for (int rowNum = 2; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    // Check if row is empty
                    bool isEmptyRow = true;
                    for (int col = 1; col <= ws.Dimension.End.Column; col++)
                    {
                        if (!string.IsNullOrWhiteSpace(ws.Cells[rowNum, col].Text))
                        {
                            isEmptyRow = false;
                            break;
                        }
                    }

                    if (isEmptyRow)
                        continue;

                    var row = dt.NewRow();

                    for (int col = 1; col <= ws.Dimension.End.Column; col++)
                    {
                        row[col - 1] = ws.Cells[rowNum, col].Text?.Trim() ?? string.Empty;
                    }

                    dt.Rows.Add(row);
                }
            }

            return dt;
        }

        private DataTable ReadCsvFileToDataTable(string path)
        {
            var dt = new DataTable();

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null,
                TrimOptions = CsvHelper.Configuration.TrimOptions.Trim,
                BadDataFound = null // Ignore bad data instead of throwing
            }))
            {
                // Read header
                csv.Read();
                csv.ReadHeader();

                foreach (var header in csv.HeaderRecord)
                {
                    dt.Columns.Add(header?.Trim() ?? string.Empty);
                }

                // Read rows
                while (csv.Read())
                {
                    // Skip empty rows
                    bool isEmptyRow = true;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(csv.GetField(i)))
                        {
                            isEmptyRow = false;
                            break;
                        }
                    }

                    if (isEmptyRow)
                        continue;

                    var row = dt.NewRow();

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row[i] = csv.GetField(i)?.Trim() ?? string.Empty;
                    }

                    dt.Rows.Add(row);
                }
            }

            return dt;
        }

        private string GetColumnValue(DataRow row, string columnName)
        {
            if (!row.Table.Columns.Contains(columnName) || row.IsNull(columnName))
                return null;

            return row[columnName]?.ToString()?.Trim();
        }

        private bool CreateFolderIfNeeded(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private JsonResult CreateJsonResult(int page, int pageSize, string message)
        {
            return Json(new
            {
                redirectTo = Url.Action("Index", "Employee", new { page, pageSize }),
                message,
                position = "mainContent"
            });
        }
        #endregion



        #endregion

        public IActionResult Excel_Download(int pageNumber = 1, int pageSize = 10)
        {

            //================================FOR EXCEL EXPORT REPORT================================
            var sFileName = "Employee Details" + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";

            using (var workbook = new XLWorkbook())
            {

                var employees = _employeeService.GetAll();
                var data = employees.Select(e => new EmployeeViewModel
                {
                    Id = e.Id,
                    EmployeeId = e.EmployeeId,
                    FullName = e.FullName,
                    Email = e.Email,
                    Designation = e.Designation,
                    IsActive = e.IsActive
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();


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
                    ws.Cell(1, headerColumn++).Value = "EmployeeId";

                    ws.Cell(1, headerColumn++).Value = "Full Name";
                    ws.Cell(1, headerColumn++).Value = "Email";
                    ws.Cell(1, headerColumn++).Value = "Designation";
                    ws.Cell(1, headerColumn++).Value = "IsActive";


                   

                }

                if (data.Any())
                {
                    int rowId = 2;

                    foreach (var singleData in data)
                    {
                        var DataColumn = 1;
                        ws.Cell(rowId, DataColumn++).Value = singleData.EmployeeId;

                        ws.Cell(rowId, DataColumn++).Value = singleData.FullName;
                        ws.Cell(rowId, DataColumn++).Value = singleData.Email;
                        ws.Cell(rowId, DataColumn++).Value = singleData.Designation;
                        ws.Cell(rowId, DataColumn++).Value = singleData.IsActive;
                       

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

    }
}

