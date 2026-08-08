using AlphaCare.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using QCMS.Repositories;
using QCMS.Services;
using RetailCare.Common;
using RetailCare.Interface;
using RetailCare.Interface.CRMInterface;
using RetailCare.Interface.ServiceInterface;
using RetailCare.Repositories;
using RetailCare.Repositories.CRMRepository;
using RetailCare.Repositories.ServiceRepository;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Add your DatabaseService and UserRepository
builder.Services.AddScoped<DatabaseService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<CommonService>();
builder.Services.AddScoped<CommonRepository>();

// Newly Added Code
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<IComplainRepository, ComplainRepository>();
builder.Services.AddScoped<ICommonMethod, SessionHelper>();
builder.Services.AddScoped<IProblemRepository, ProblemRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICommonServiceMethods, CommonServiceMethods>();
builder.Services.AddScoped<ITechnicianRepository, TechnicianRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IZoneRepository, ZoneRepository>();
builder.Services.AddScoped<IAssignmentManagementRepository, AssignmentManagementRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IReportGenerationRepository, ReportGenerationRepository>();
builder.Services.AddScoped<IReportingMethods, ReportingMethods>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IUserManagementRepository, UserManagementRepository>();

// Add HttpClient and ApiService
builder.Services.AddHttpClient<ApiService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});

// ===============================
// Authentication (Cookie)
// ===============================
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Accounts/Login";
        options.AccessDeniedPath = "/Accounts/Denied";

        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // match session
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

RotativaConfiguration.Setup(
    Path.Combine(app.Environment.WebRootPath, "Rotativa")
);
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UsePathBase("/HPSWeb");
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable session before authorization
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// ===============================
// DEBUG MIDDLEWARE - LOG COOKIES
// ===============================
app.Use(async (context, next) =>
{
    // Log all cookies
    var cookies = context.Request.Cookies;
    Console.WriteLine("=== REQUEST COOKIES ===");
    foreach (var cookie in cookies)
    {
        Console.WriteLine($"Cookie: {cookie.Key} = {cookie.Value}");
    }
    Console.WriteLine("========================");
    await next();
});

app.UseAuthorization();

// ===============================
// SESSION EXPIRY MIDDLEWARE
// ===============================
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower();

    // Skip public paths
    if (path != null &&
        (path.StartsWith("/accounts/login") ||
         path.StartsWith("/accounts/logout") ||
         path.StartsWith("/css") ||
         path.StartsWith("/js") ||
         path.StartsWith("/images")))
    {
        await next();
        return;
    }

    if (context.User.Identity?.IsAuthenticated == true)
    {
        var user = context.Session.GetString("USERID");

        // Session expired but cookie still exists
        if (string.IsNullOrEmpty(user))
        {
            await context.SignOutAsync();
            context.Response.Redirect("/Accounts/Login");
            return;
        }
    }

    await next();
});
// AREA ROUTE (if you have Areas)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Accounts}/{action=Login}/{id?}"
);

app.MapControllers(); // enable API endpoints

app.Run();
