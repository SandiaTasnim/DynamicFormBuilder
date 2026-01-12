namespace DynamicFormBuilder.Models
{
    public class EmployeeChangeHistoriesModel
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string PreviousData { get; set; }
        public string UpdatedData { get; set; }
        public DateTime ChangedAt { get; set; }


    }
}
