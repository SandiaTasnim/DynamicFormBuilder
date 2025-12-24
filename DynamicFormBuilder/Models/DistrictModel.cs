using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("District")]

public class District
{
    [Key]
    public int DistrictID { get; set; }

    [Required]
    [StringLength(100)]
    public string DistrictName { get; set; }

    // Foreign Key
    public int DivisionID { get; set; }

    // Navigation
    public Division Division { get; set; }
    public ICollection<Customer> Customers { get; set; }
}
