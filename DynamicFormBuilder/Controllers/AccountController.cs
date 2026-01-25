using DynamicFormBuilder.ViewModels;
using Microsoft.AspNetCore.Authorization;

// Explicitly specify the correct namespace to resolve ambiguity
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private ApplicationDbContext _context;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public IActionResult Register()
    {
        if (User.Identity.IsAuthenticated)
            return RedirectToAction("Index", "Home");

        return View(new RegisterViewModel());
    }

    [HttpPost]
    public IActionResult Register(RegisterViewModel model)
    {
        if (User.Identity.IsAuthenticated)
            return RedirectToAction("Index", "Home");

        if (!ModelState.IsValid)
            return View(model);

        if (_context == null)
            throw new Exception("DbContext is NULL");

        bool isAllowedUser =
            _context.Students != null && _context.Students.Any(s => s.Email == model.Email) ||
            _context.Employees != null && _context.Employees.Any(e => e.Email == model.Email) ||
            _context.Customers != null && _context.Customers.Any(c => c.Email == model.Email);

        if (!isAllowedUser)
        {
            ModelState.AddModelError("", "You are not authorized to register.");
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        var result = _userManager.CreateAsync(user, model.Password).Result;

        if (result.Succeeded)
        {
            TempData["success"] = "Registration successful. Please login.";
            return RedirectToAction("Login");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(model);
    }



    //[HttpPost]
    //public async Task<IActionResult> Register(RegisterViewModel model)
    //{
    //    if (User.Identity.IsAuthenticated)
    //    {
    //        return RedirectToAction("Index", "Home");
    //    }

    //    if (!ModelState.IsValid)
    //        return View(model);

    //    // 🔒 CHECK: User must exist in Student / Employee / Customer
    //    bool isAllowedUser =
    //        _context.Students.Any(s => s.Email == model.Email) ||
    //        _context.Employees.Any(e => e.Email == model.Email) ||
    //        _context.Customers.Any(c => c.Email == model.Email);

    //    if (!isAllowedUser)
    //    {
    //        ModelState.AddModelError("",
    //            "You are not authorized to register. Please contact administration.");
    //        return View(model);
    //    }
    //    if (!ModelState.IsValid)
    //        return View(model);

    //    var user = new ApplicationUser
    //    {
    //        UserName = model.Email,
    //        Email = model.Email,
    //        FirstName = model.FirstName,
    //        LastName = model.LastName
    //    };

    //    //var result = await _userManager.CreateAsync(user, model.Password);
    //    var result = _userManager.CreateAsync(user, model.Password).Result;


    //    if (result.Succeeded)
    //    {
    //        TempData["success"] = "Registration successful. Please login.";
    //        return RedirectToAction("Login");
    //    }

    //    foreach (var error in result.Errors)
    //        ModelState.AddModelError(string.Empty, error.Description);

    //    return View(model);
    //}

    [HttpGet]
    [AllowAnonymous]

    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
            // or return RedirectToAction("Index", "Form"); depending on your home page
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        // Redirect to dashboard if already logged in
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        if (!ModelState.IsValid)
            return View(model);

        var result = await _signInManager.PasswordSignInAsync(
            model.Email,   // must match UserName
            model.Password,
          
            isPersistent: false,
            lockoutOnFailure: true);

        if (result.Succeeded)
            return RedirectToAction("Index", "Home");

        ModelState.AddModelError(string.Empty, "Invalid login attempt");
        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

    [Authorize]
    public IActionResult Profile()
    {
        var userId = _userManager.GetUserId(User);

        var user = _userManager.Users
            .Where(u => u.Id == userId)
            .Select(u => new ProfileViewModel
            {
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName

            })
            .FirstOrDefault();

        if (user == null)
            return NotFound();

        return View(user);
    }

    [Authorize]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = _userManager.GetUserAsync(User).Result;

        if (user == null)
            return RedirectToAction("Login");

        var result = _userManager.ChangePasswordAsync(
            user,
            model.CurrentPassword,
            model.NewPassword
        ).Result;

        if (result.Succeeded)
        {
            TempData["success"] = "Password changed successfully.";
            return RedirectToAction("Profile");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
    }



}