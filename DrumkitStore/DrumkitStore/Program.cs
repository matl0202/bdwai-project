using DrumkitStore.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

//dodawanie bazy
builder.Services.AddDbContext<DrumkityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//uwierzytelnianie itd
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login"; //do logowania
        options.AccessDeniedPath = "/Account/AccessDenied"; //jak brak uprawnien
    });

builder.Services.AddAuthorization();

var app = builder.Build();

//do errorow cos znow
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();//uwierzytel
app.UseAuthorization(); //autoryzacja

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Drumkits}/{action=Index}/{id?}");

app.Run();