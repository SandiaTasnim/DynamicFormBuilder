//using DynamicFormBuilder.Data;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();
//builder.Services.AddScoped<FormRepository>();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//}
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Form}/{action=Index}/{id?}");

//app.Run();

//using DynamicFormBuilder.Data;
//using DynamicFormBuilder.Data.DBContext;
//using DynamicFormBuilder.Services;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);
//// Register DbContext
//builder.Services.AddDbContext<ApplicationDBContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//// Add services to the container.
//builder.Services.AddControllersWithViews();
//builder.Services.AddScoped<FormRepository>();
//builder.Services.RegisterApplicationServices();
//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//}
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Form}/{action=Index}/{id?}");

//app.Run();


using DynamicFormBuilder.Data;
using DynamicFormBuilder.Services;
using DynamicFormBuilder.Services.Implementations;
using DynamicFormBuilder.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;



var builder = WebApplication.CreateBuilder(args);

// Register DbContext
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICustomerService, CustomerService>();


// Register services
builder.Services.RegisterApplicationServices();
//builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<FormRepository>();



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Customer}/{action=Index}/{id?}");

app.Run();
