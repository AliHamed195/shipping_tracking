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
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<MyDbContext>()
.AddDefaultTokenProviders();


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
