using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Practical19.Data;
using Practical19.Models;


var builder = WebApplication.CreateBuilder(args);

// Configure database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity using ApplicationUser
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Add MVC services
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Apply migrations and seed roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ApplicationDbContext>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<User>>();

    dbContext.Database.Migrate(); // Ensure the latest migration is applied
    
    var seedingData = new SeedingData();
    seedingData.SeedRolesAsync(roleManager, userManager).GetAwaiter().GetResult();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Function to seed roles
