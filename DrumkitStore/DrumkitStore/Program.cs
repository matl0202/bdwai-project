using DrumkitStore.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Dodaj usługi MVC
builder.Services.AddControllersWithViews();

// Dodaj DbContext (upewnij się, że jest poprawny)
builder.Services.AddDbContext<DrumkityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dodaj uwierzytelnianie i autoryzację
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login"; // Ścieżka do strony logowania
        options.AccessDeniedPath = "/Account/AccessDenied"; // Ścieżka, jeśli brak uprawnień
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); //Uwierzytelnianie
app.UseAuthorization(); //Autoryzacja

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();