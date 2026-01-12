using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DynamicFormBuilder.ViewModels
{
    public class EmployeeViewModel
    {
        public List<EmployeeModel> Employees { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalRecords / PageSize);
        [Key]
        public string Id { get; set; }


        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100)]
        public string FullName { get; set; }
        public string EmployeeId { get; set; }



        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+com$", ErrorMessage = "Email must end with .com")]
        public string Email { get; set; }





        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

        [StringLength(150)]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "Designation cannot contain numbers")]
        public string Designation { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; internal set; }
        public DateTime UpdateDate { get; internal set; }
        public SelectList StatusList { get; internal set; }
    }
}
