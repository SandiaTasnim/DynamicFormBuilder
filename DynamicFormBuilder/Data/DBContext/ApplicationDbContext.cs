using DynamicFormBuilder.Models;
using ISMS.Web.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser>
{
    internal readonly object Employee;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    //public DbSet<Student> EmployeeModels { get; set; }  // <- this is required
    public DbSet<Customer> Customers { get; set; }  // <- this is required
    public DbSet<Division> Divisions { get; set; }
    public DbSet<District> Districts { get; set; }

    public DbSet<EmployeeModel> Employees { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<EmployeeChangeHistoriesModel> EmployeeChangeHistories { get; set; }

}





