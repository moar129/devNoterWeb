using devNoter.EFDbContext;
using devNoter.Models;
using devNoter.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
//using devNoter.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();


// Add DbContext and Identity
builder.Services.AddDbContext<DevNoterDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;           // Require at least one number
    options.Password.RequiredLength = 8;           // Minimum 8 characters
    options.Password.RequireNonAlphanumeric = false; // No special symbols required
    options.Password.RequireUppercase = true;      // At least one uppercase letter
    options.Password.RequireLowercase = true;      // At least one lowercase letter
    
    

    // No email confirmation required for sign in
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = false;
})
.AddEntityFrameworkStores<DevNoterDbContext>();




// Configure authentication cookie lifetime here
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);  // Cookie valid for 30 minutes
    options.SlidingExpiration = true;                   // Reset timer when active
    options.LoginPath = "/Identity/Account/Login";       // Redirect if not logged in
    options.LogoutPath = "/Identity/Account/Logout";     // Logout redirect
});


// Add DbContext and Services
builder.Services.AddScoped<dbService<LanguageFolder>>();
builder.Services.AddScoped<LanguageFolderService>();
builder.Services.AddScoped<dbService<Note>>();
builder.Services.AddScoped<NoteService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

