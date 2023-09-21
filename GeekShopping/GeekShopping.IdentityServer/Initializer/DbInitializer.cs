using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Models;
using GeekShopping.IdentityServer.Models.Context;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Security.Claims;

namespace GeekShopping.IdentityServer.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly MySQLContext _context;
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<IdentityRole> _role;

        public DbInitializer(MySQLContext context, 
            UserManager<ApplicationUser> user, 
            RoleManager<IdentityRole> role)
        {
            _context = context;
            _user = user;
            _role = role;
        }

        public void Initialize()
        {
            if (_role.FindByNameAsync(IdentityConfiguration.Admin).Result != null)
            {
                return;
            }
            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Admin)).GetAwaiter().GetResult();
            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Client)).GetAwaiter().GetResult();

            ApplicationUser admin = new ApplicationUser() 
            {
                UserName = "Leandro-admin",
                Email = "leandro-admin@erudio.com.br",
                EmailConfirmed = true,
                PhoneNumber = "+55 (11) 91234-4567",
                FirstName = "Leandro",
                LastName = "Admin"
            };

            _user.CreateAsync(admin, "Erudio123$").GetAwaiter().GetResult();
            _user.AddToRoleAsync(admin, IdentityConfiguration.Admin).GetAwaiter().GetResult();

            Claim claim = new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}" );
            var adminClaims = _user.AddClaimsAsync(admin, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                new Claim(JwtClaimTypes.GivenName, $"{admin.FirstName}"),
                new Claim(JwtClaimTypes.FamilyName, $"{admin.LastName}"),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin)
            }).Result;

            ApplicationUser client = new ApplicationUser()
            {
                UserName = "Leandro-client",
                Email = "leandro-client@erudio.com.br",
                EmailConfirmed = true,
                PhoneNumber = "+55 (11) 91234-4567",
                FirstName = "Leandro",
                LastName = "client"
            };

            _user.CreateAsync(client, "Erudio123$").GetAwaiter().GetResult();
            _user.AddToRoleAsync(client, IdentityConfiguration.Client).GetAwaiter().GetResult();

            //Claim claim = new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}");
            var clientClaims = _user.AddClaimsAsync(client, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
                new Claim(JwtClaimTypes.GivenName, $"{client.FirstName}"),
                new Claim(JwtClaimTypes.FamilyName, $"{client.LastName}"),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Client)
            }).Result;
        }
    }
}
