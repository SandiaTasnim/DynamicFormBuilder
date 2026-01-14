using System.ComponentModel.DataAnnotations;

namespace DynamicFormBuilder.ViewModels
{
    public class EmployeeChangeHistoriesViewModel
    {
        [Key]
        public Guid Id { get; set; }   
        public string EmployeeId { get; set; }
        public string PreviousData { get; set; }
        public string UpdatedData { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
