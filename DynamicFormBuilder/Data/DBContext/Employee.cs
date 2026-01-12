
public class Employee
{
    internal string FullName;
    internal string Designation;
    internal DateTime? DOB;
    internal string Status;
    internal string Email;

    public string EmployeeId { get; set; }
    public string Id { get; internal set; }
}