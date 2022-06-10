using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SnackBar.Services.IdentityServer;
using SnackBar.Services.IdentityServer.DbContext;
using SnackBar.Services.IdentityServer.Models;
using SnackBar.Services.IdentityServer.Services;
using SnackBar.Services.IdentityServices.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
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

//add ProfileService as dependecy injection to ccreate object and call the methods in profile service, 
//so that it will add custom claims to profile

builder.Services.AddScoped<IProfileService,ProfileService>();


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

app.UseIdentityServer();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    // use dbInitializer
    dbInitializer.Initialize();
}

app.MapRazorPages();

app.Run();
