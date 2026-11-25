using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OFFSTEP.Data;
using OFFSTEP.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("Set ConnectionStrings:DefaultConnection in user-secrets or appsettings.json.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
}).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(100); options.Cookie.HttpOnly = true; options.Cookie.IsEssential = true; });
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<OrderEmailService>();

var app = builder.Build();
if (!app.Environment.IsDevelopment()) { app.UseExceptionHandler("/Store/Error"); app.UseHsts(); }
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Store}/{action=Index}/{id?}");
app.MapRazorPages();

// Seed demo catalogue and optional administrator role. Run migrations first.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        if (db.Database.CanConnect())
        {
            var roles = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (!await roles.RoleExistsAsync("Admin")) await roles.CreateAsync(new IdentityRole("Admin"));
            var adminEmail = app.Configuration["Admin:Email"];
            if (!string.IsNullOrWhiteSpace(adminEmail))
            {
                var users = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var admin = await users.FindByEmailAsync(adminEmail);
                if (admin != null && !await users.IsInRoleAsync(admin, "Admin")) await users.AddToRoleAsync(admin, "Admin");
            }
            await SeedData.InitializeAsync(db);
        }
    }
    catch (Exception ex) { app.Logger.LogError(ex, "Database initialization skipped. Apply EF Core migrations and verify the SQL Server connection."); }
}
app.Run();
