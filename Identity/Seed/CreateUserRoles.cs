using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Identity.Authorization;
using Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Seed
{
    public static class CreateUserRoles
    {
        public static async Task SeedAsync(BasketCommerceIdentityDbContext _context)
        {
            // Super Admin Role
            if (!await _context.UserRoles.AnyAsync(x => x.UserId == IdConstants.SuperAdminId && x.RoleId == IdConstants.SuperAdminRoleId))
            {
                await _context.UserRoles.AddAsync(new IdentityUserRole<string>
                {
                    UserId = IdConstants.SuperAdminId,
                    RoleId = IdConstants.SuperAdminRoleId,
                });
            }

            // Admin Role
            if (!await _context.UserRoles.AnyAsync(x => x.UserId == IdConstants.AdminId && x.RoleId == IdConstants.AdminRoleId))
            {
                await _context.UserRoles.AddAsync(new IdentityUserRole<string>
                {
                    UserId = IdConstants.AdminId,
                    RoleId = IdConstants.AdminRoleId,
                });
            }
            _context.SaveChanges();
        }
    }
}
