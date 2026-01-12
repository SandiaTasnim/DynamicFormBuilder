using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

[Table("Customer")]
public class Customer
{
    [Key]
    public int CustomerID { get; set; }

    [Required(ErrorMessage = "First Name is required")]
    [StringLength(100)]
    public string FirstName { get; set; }

    [StringLength(100)]
    public string LastName { get; set; }

    [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone number must be exactly 11 digits")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be numeric and 11 digits")]
    public string Phone { get; set; }

    [StringLength(255)]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+com$", ErrorMessage = "Email must end with .com")]
    public string Email { get; set; }

    [Required(ErrorMessage = "NID is required")]
    [RegularExpression(@"^\d{13}$", ErrorMessage = "NID must be exactly 13 digits")]
    public string NID { get; set; }

    // Foreign Keys
    public int? DivisionID { get; set; }
    public int? DistrictID { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DOB { get; set; }

    [StringLength(150)]
    [RegularExpression(@"^[^\d]+$", ErrorMessage = "Profession cannot contain numbers")]
    public string Profession { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Balance { get; set; }
    
}
