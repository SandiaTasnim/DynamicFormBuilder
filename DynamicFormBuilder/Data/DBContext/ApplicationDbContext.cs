using DynamicFormBuilder.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<StudentModel> StudentModels { get; set; }  // <- this is required
    public DbSet<Customer> Customers { get; set; }  // <- this is required
    public DbSet<Division> Divisions { get; set; }
    public DbSet<District> Districts { get; set; }
}
