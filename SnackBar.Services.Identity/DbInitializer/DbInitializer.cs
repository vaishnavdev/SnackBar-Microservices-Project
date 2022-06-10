using IdentityModel;
using Microsoft.AspNetCore.Identity;
using SnackBar.Services.Identity.Data;
using SnackBar.Services.Identity.Models;
using System.Security.Claims;

namespace SnackBar.Services.Identity.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context, 
                             UserManager<ApplicationUser> usermanager, 
                             RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _usermanager = usermanager;
            _roleManager = roleManager;
        }
        public async void Initialize()
        {
            if(_roleManager.FindByNameAsync(SD.Admin).Result == null)
            {
                //if Admin role is not present in db the see the db 
                _roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
            }
            else
            { return;}
            ApplicationUser adminUser = new ApplicationUser()
            {
                UserName = "admin1@gmail.com",
                Email = "admin1@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "8919552778",
                Firstname = "Vaishnav",
                Lastname = "Kannan"
            };
            _usermanager.CreateAsync(adminUser,"Race@race1").GetAwaiter().GetResult();
            _usermanager.AddToRoleAsync(adminUser, SD.Admin).GetAwaiter().GetResult();
            var temp = _usermanager.AddClaimsAsync(adminUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name,adminUser.Firstname+" "+adminUser.Lastname),
                new Claim(JwtClaimTypes.GivenName,adminUser.Firstname),
                new Claim(JwtClaimTypes.FamilyName,adminUser.Firstname),
                new Claim(JwtClaimTypes.Role,SD.Admin),
            }).Result;

            ApplicationUser customerUser = new ApplicationUser()
            {
                UserName = "customer1@gmail.com",
                Email = "customer1@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "8919552778",
                Firstname = "Vaishnav",
                Lastname = "Kannan"
            };
            _usermanager.CreateAsync(customerUser, "Race@race1").GetAwaiter().GetResult();
            _usermanager.AddToRoleAsync(customerUser, SD.Customer).GetAwaiter().GetResult();
            var temp1 = _usermanager.AddClaimsAsync(customerUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name,customerUser.Firstname+" "+customerUser.Lastname),
                new Claim(JwtClaimTypes.GivenName,customerUser.Firstname),
                new Claim(JwtClaimTypes.FamilyName,customerUser.Firstname),
                new Claim(JwtClaimTypes.Role,SD.Customer),
            }).Result;
        }
    }
}
