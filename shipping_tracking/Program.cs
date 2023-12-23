using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using shipping_tracking.Models;
using shipping_tracking.Services.Interfaces;
using shipping_tracking.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("myconn") ?? throw new InvalidOperationException("Connection string 'myconn' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();
builder.Services.AddDbContext<MyDbContext>(item => item.UseSqlServer(configuration.GetConnectionString("myconn")));

// Identity Managers
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.SignIn.RequireConfirmedAccount = false; // to avoid the email confirmation ...
})
.AddEntityFrameworkStores<MyDbContext>()
.AddDefaultTokenProviders();

// Register Session services
builder.Services.AddSession(options =>
{
    // If the 20 minutes end without any action in the page the user will log out.
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    // need to read more about the lines below ...
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Add SessionStateTempDataProvider
builder.Services.AddControllersWithViews().AddSessionStateTempDataProvider();

// Configure Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Home/Error"; // or create page for "/Account/AccessDenied";
    });

// Services & Repositories
builder.Services.AddScoped<IPasswordService, PasswordService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use the session middleware
app.UseSession();

//// Check for session validity and redirect
//app.Use(async (context, next) =>
//{
//    if (!context.Session.Keys.Contains("UserName")
//        && !context.Request.Path.StartsWithSegments("/Account/Login")
//        && !context.Request.Path.StartsWithSegments("/Account/Register")
//        && !context.Request.Path.StartsWithSegments("/Account/Logout")
//        && !context.Request.Path.StartsWithSegments("/Home/HomePage"))
//    {
//        context.Response.Redirect("/Account/Logout");
//        return;
//    }

//    await next.Invoke();
//});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=HomePage}/{id?}");

app.Run();
