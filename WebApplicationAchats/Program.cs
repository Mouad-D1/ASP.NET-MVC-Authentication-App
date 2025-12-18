using Microsoft.EntityFrameworkCore;
using WebApplicationAchats.Models;
using Microsoft.AspNetCore.Identity;
using WebApplicationAchats.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DB Métier
builder.Services.AddDbContext<VenteContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VenteDb")));

// DB Auth
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthDb")));

builder.Services.AddRazorPages();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

var app = builder.Build()
    ;
// Création automatique des rôles + Admin + Manager
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string[] roles = { "Admin", "Manager", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // Compte admin par défaut
    var adminEmail = "admin@emsi.ma";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
        await userManager.AddToRoleAsync(adminUser, "Admin");

    // Compte manager pour test
    // Compte manager pour test
    var managerEmail = "manager@emsi.ma";
    var managerUser = await userManager.FindByEmailAsync(managerEmail);
    if (managerUser == null)
    {
        managerUser = new IdentityUser
        {
            UserName = managerEmail,
            Email = managerEmail,
            EmailConfirmed = true
        };
        await userManager.CreateAsync(managerUser, "Manager@123");
        await userManager.AddToRoleAsync(managerUser, "Manager");
    
}
}





// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapRazorPages();



app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
