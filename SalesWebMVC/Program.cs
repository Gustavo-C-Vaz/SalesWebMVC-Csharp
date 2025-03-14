using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMVC.Controllers;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using SalesWebMVC.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SalesWebMVCContext>(options =>
    options.UseMySql(builder
    .Configuration
    .GetConnectionString("SalesWebMVCContext"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("SalesWebMVCContext")))); // MySQL -------

//options.UseSqlServer(builder.Configuration.GetConnectionString("SalesWebMVCContext")
//?? throw new InvalidOperationException("Connection string 'SalesWebMVCContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<SellerService>();  // SellerService -----
builder.Services.AddScoped<DepartmentService>();  // DepartmentService -----
builder.Services.AddScoped<SalesRecordService>();  // SalesRecordService  ------

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var seedingService = new SeedingService(services.GetRequiredService<SalesWebMVCContext>());
seedingService.Seed();  // SeedingService -----------


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
