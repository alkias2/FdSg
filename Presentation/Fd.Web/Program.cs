using EFCoreSandbox;
using Fd.Core.Infrastructure;
using Fd.Data;
using Microsoft.EntityFrameworkCore;
using System;
using Fd.Services.StormGlass;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddScoped<ITypeFinder, AppDomainTypeFinder>();
builder.Services.AddScoped<IStormGlassService, StormGlassService>();

var connectionString = builder.Configuration["ConnectionStrings:DbConnection"];

builder.Services.AddDbContext<DataContext>(options =>
{
	options.UseSqlServer(builder.Configuration["ConnectionStrings:DbConnection"]);
});





// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
SeedData.SeedDatabase(context);

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
		pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
