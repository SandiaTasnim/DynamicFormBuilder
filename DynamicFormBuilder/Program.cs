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
//using DynamicFormBuilder.Services;
//using DynamicFormBuilder.Services.Implementations;
//using DynamicFormBuilder.Services.Interfaces;
//using Microsoft.EntityFrameworkCore;
//using OfficeOpenXml;
//var builder = WebApplication.CreateBuilder(args);
//// DbContext
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//// MVC
//builder.Services.AddControllersWithViews();
//// Services
//builder.Services.AddScoped<ICustomerService, CustomerService>();
//builder.Services.RegisterApplicationServices();
//builder.Services.AddScoped<FormRepository>();
//var app = builder.Build();
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//}
//app.UseStaticFiles();
//app.UseRouting();
//// ❌ REMOVE Authorization (no login system exists)
////app.UseAuthorization();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Employee}/{action=Index}/{id?}");
//app.Run();




using DynamicFormBuilder.Data;
using DynamicFormBuilder.Models;
using DynamicFormBuilder.Services;
using DynamicFormBuilder.Services.Implementations;
using DynamicFormBuilder.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity (ADD ONLY ONCE)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Cookie config ✅ SAFE to keep
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.SlidingExpiration = true;
});

// MVC
builder.Services.AddControllersWithViews();

// App services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.RegisterApplicationServices();
builder.Services.AddScoped<FormRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

// Authentication pipeline (CORRECT ORDER)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
