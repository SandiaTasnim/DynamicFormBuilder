using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

[Table("Employee")]
public class EmployeeModel 
{
    [Key]
    public string? Id { get; set; }

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
    [Required(ErrorMessage = "Status is required")]
    public bool IsActive{ get; set; }
   
    

    [ValidateNever]
    [NotMapped]
    public SelectList StatusList { get; internal set; }
}

