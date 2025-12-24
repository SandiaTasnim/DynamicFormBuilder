using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Division")]
public class Division
{
    [Key]
    public int DivisionID { get; set; }

    [Required]
    [StringLength(100)]
    public string DivisionName { get; set; }

    public ICollection<District> Districts { get; set; }
    public ICollection<Customer> Customers { get; set; }
}
