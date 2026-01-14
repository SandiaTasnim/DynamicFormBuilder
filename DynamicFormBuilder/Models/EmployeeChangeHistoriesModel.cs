using System.ComponentModel.DataAnnotations;

namespace DynamicFormBuilder.Models
{
    public class EmployeeChangeHistoriesModel
    {
        [Key]
        public Guid Id { get; set; }   // ✅ uniqueidentifier
        public string EmployeeId { get; set; }
        public string PreviousData { get; set; }
        public string UpdatedData { get; set; }
        public DateTime ChangedAt { get; set; }


    }
}
