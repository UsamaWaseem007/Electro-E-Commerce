using MartWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext for Identity
builder.Services.AddDbContext<MartWebAppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
	.AddEntityFrameworkStores<MartWebAppDbContext>()
	.AddDefaultTokenProviders();

// Add session services
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(1); // Set session timeout to 1 minute for inactivity
	options.Cookie.HttpOnly = true; // Make the cookie accessible only via HTTP
	options.Cookie.IsEssential = true; // Needed for non-essential cookies
});

// Configure application cookie
builder.Services.ConfigureApplicationCookie(options =>
{
	options.AccessDeniedPath = "/Home/AccessDenied"; // Common access denied path
	options.Events.OnRedirectToAccessDenied = context =>
	{
		context.Response.Redirect("/Home/AccessDenied");
		return Task.CompletedTask;
	};

	options.Events.OnRedirectToLogin = context =>
	{
		if (context.HttpContext.User.IsInRole("Admin"))
		{
			context.Response.Redirect("/Account/AdminLogin"); // Admin login path
		}
		else
		{
			context.Response.Redirect("/Account/Login"); // User login path
		}
		return Task.CompletedTask;
	};
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICartService, CartService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); // Ensure session is enabled before authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();





// OLD Code But Working


//using MartWebApp.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);

//// Add DbContext for Identity (Replace 'YourDbContext' with your context name)
//builder.Services.AddDbContext<MartWebAppDbContext>(options =>
//	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//// Add Identity services
//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//	.AddEntityFrameworkStores<MartWebAppDbContext>()
//	.AddDefaultTokenProviders();
////add session service
//builder.Services.AddSession();
//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.AccessDeniedPath = "/Home/AccessDenied"; // Common access denied path

//    options.Events.OnRedirectToAccessDenied = context =>
//    {
//        // Redirect to access denied page
//        context.Response.Redirect("/Home/AccessDenied");
//        return Task.CompletedTask;
//    };

//    // Custom logic for determining the login path based on the role
//    options.Events.OnRedirectToLogin = context =>
//    {
//        // Check if the user is trying to access an admin route
//        if (context.HttpContext.User.IsInRole("Admin"))
//        {
//            context.Response.Redirect("/Account/AdminLogin"); // Admin login path
//        }
//        else
//        {
//            context.Response.Redirect("/Account/Login"); // User login path
//        }
//        return Task.CompletedTask;
//    };
//});

//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true; // Needed for non-essential cookies
//});
//// Add services to the container.
//builder.Services.AddControllersWithViews();

//builder.Services.AddDbContext<MartWebAppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddScoped<ICartService, CartService>();





//var app = builder.Build();




//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();
//app.UseSession();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();
