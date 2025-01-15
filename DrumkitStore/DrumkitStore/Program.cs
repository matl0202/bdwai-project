using DrumkitStore.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DrumkityDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));
//do naszego serwisu dodajemy ten context co przed chwila zbudowalismy z uzyciem sql server i potem jego konfiguracja zbudowana w jsonie

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Drumkit}/{action=Index}/{id?}");

app.Run();
