using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicFormBuilder.ViewModels
{
    public class CustomerViewModel
    {
        public int CustomerID { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(30)]
        public string Phone { get; set; }
        public string DivisionName { get; set; }

        public string DistrictName { get; set; }


        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50)]
        public string NID { get; set; }

        // Foreign Keys
        public int? DivisionID { get; set; }
        public int? DistrictID { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime? DOB { get; set; }

        [StringLength(150)]
        public string Profession { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
    }
}
