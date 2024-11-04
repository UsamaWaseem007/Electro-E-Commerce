using MartWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class AccountController : Controller
{
	private readonly UserManager<IdentityUser> _userManager;
	private readonly SignInManager<IdentityUser> _signInManager;

	public AccountController(MartWebAppDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
	{
		_context = context;
		_userManager = userManager;
		_signInManager = signInManager;
	}
	private readonly MartWebAppDbContext _context;

	[HttpGet]
	public IActionResult Adminlogin()
	{
		return View();
	}

	
	public async Task<IActionResult> Logout()
    { // Logout the user
        await _signInManager.SignOutAsync();

        // Optionally clear the session
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home"); // Ya kisi aur page par redirect karein

	}
	[HttpPost]
	public async Task< IActionResult> Adminlogin( LoginViewModel login)
	{
		if (ModelState.IsValid)
		{
			var user = await _userManager.FindByEmailAsync(login.Email);
			if( user != null && await _userManager.IsInRoleAsync(user, "Admin"))
			{
				var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, false);
				if (result.Succeeded)
				{
					HttpContext.Session.SetString("AdminEmail", login.Email);
					HttpContext.Session.SetString("AdminPassword", login.Password);
					return RedirectToAction("Index", "AdminProduct");
				}
			}

		}
		else
		{
            TempData["Invalid Details"] = "Invalid Admin !!";
        }
      
        return View();
	}


	public async Task<IActionResult> AdminLogout()
	{

		await _signInManager.SignOutAsync(); // User ko logout karein
		HttpContext.Session.Clear(); // Session clear karein

		
		
		return RedirectToAction("index", "Home");
	}


	public IActionResult RegisterAdmin()
	{
		return View();
	}

    [HttpPost]
    public async Task<IActionResult> RegisterAdmin(AdminRegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // User ko role assign karein agar aap roles use kar rahe hain
                await _userManager.AddToRoleAsync(user, "Admin");

                return RedirectToAction("Index", "AdminProduct"); // Ya kisi aur page par redirect karein
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }




    [HttpGet]
	public IActionResult Login()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Login(LoginViewModel model)
	{
		if (ModelState.IsValid)
		{
			var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
			if (result.Succeeded)
			{
                HttpContext.Session.SetString("UserEmail", model.Email);
                HttpContext.Session.SetString("UserPassword", model.Password);
               
                return RedirectToAction("Index", "Home");
			}
			TempData["invalid"] = "Invalid User Name Or Password";
		}
		return View(model);
	}


	[HttpGet]
	public IActionResult Register()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Register(RegisterViewModel model)
	{
		if (ModelState.IsValid)
		{
			var user = new IdentityUser { UserName = model.Email, Email = model.Email };
			var result = await _userManager.CreateAsync(user, model.Password);
			if (result.Succeeded)
			{
				await _signInManager.SignInAsync(user, isPersistent: false);
                TempData["Registration"] = "Registration Succeeded!";
                return RedirectToAction("Login", "Account");

			}
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}

		return View(model);
	}


}
