using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using SnackBar.Services.IdentityServer.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace SnackBar.Services.IdentityServer.Services
{
    public class ProfileService : IProfileService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;

        public ProfileService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //get sthe user id of the logged in user
            string subject = context.Subject.GetSubjectId();
            ApplicationUser user = await _userManager.FindByIdAsync(subject);
            ClaimsPrincipal userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);

            List<Claim> claims = userClaims.Claims.ToList();
            //whatever claim types are requested we will find those and assign to this list of claims
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            claims.Add(new Claim(JwtClaimTypes.FamilyName, user.Lastname));
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.Firstname));
            if (_userManager.SupportsUserRole)
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);
                foreach (var roleName in roles)
                {
                    claims.Add(new Claim(JwtClaimTypes.Role, roleName));
                    //we have not addded claims to role, but if we  add claims to role then we can configure like this
                    if (_roleManager.SupportsRoleClaims)
                    {
                        IdentityRole role = await _roleManager.FindByIdAsync(roleName);
                        if(role != null)
                        {
                            claims.AddRange(await _roleManager.GetClaimsAsync(role));
                        }
                    }
                }
            }
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            string sub = context.Subject.GetSubjectId();
            ApplicationUser user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }

}