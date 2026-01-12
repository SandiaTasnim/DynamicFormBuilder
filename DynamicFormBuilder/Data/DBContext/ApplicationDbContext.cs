using DynamicFormBuilder.Models;
using ISMS.Web.Areas.Admin.Controllers;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    internal readonly object Employee;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<StudentModel> EmployeeModels { get; set; }  // <- this is required
    public DbSet<Customer> Customers { get; set; }  // <- this is required
    public DbSet<Division> Divisions { get; set; }
    public DbSet<District> Districts { get; set; }

    public DbSet<EmployeeModel> Employees { get; set; }
    public DbSet<EmployeeChangeHistoriesModel> EmployeeChangeHistories { get; set; }

    }
