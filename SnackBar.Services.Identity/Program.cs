using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SnackBar.Services.Identity;
using SnackBar.Services.Identity.Data;
using SnackBar.Services.Identity.DbInitializer;
using SnackBar.Services.Identity.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//configure database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//add identity server
var builder1 = builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitStaticAudienceClaim = true;
}).AddInMemoryIdentityResources(SD.IdentityResources)
.AddInMemoryApiScopes(SD.ApiScopes)
.AddInMemoryClients(SD.clients)
.AddAspNetIdentity<ApplicationUser>();

builder1.AddDeveloperSigningCredential();

//configure the database seeding everytime when app runs..create DbInitializer object for Dependency Injection
builder.Services.AddScoped<IDbInitializer, DbInitializer>();


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
app.UseIdentityServer();

app.UseAuthorization();

//initialize Db Initializer app.Services.GetService<IDbInitializer>();
using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    // use dbInitializer
    dbInitializer.Initialize();
}
//app.MapRazorPages().RequireAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
