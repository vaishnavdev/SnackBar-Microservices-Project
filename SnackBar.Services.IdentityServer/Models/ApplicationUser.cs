using Microsoft.AspNetCore.Identity;

namespace SnackBar.Services.IdentityServer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
