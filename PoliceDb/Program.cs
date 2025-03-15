using Microsoft.EntityFrameworkCore;
using PoliceDb.Data;
using PoliceDb.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PoliceDbContext>(
    options =>
    {
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("Default")
            );
    });

builder.Services.AddScoped<PoliceServices>();

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
    pattern: "{controller=Police}/{action=Index}/{id?}");

app.Run();
