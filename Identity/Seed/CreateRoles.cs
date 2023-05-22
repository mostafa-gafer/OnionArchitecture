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
    public static class CreateRoles
    {
        public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
        {
            if ( !await roleManager.Roles.AnyAsync(r => r.Name == NameConstants.SuperAdminRole))
            {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Id = IdConstants.SuperAdminRoleId,
                    Name = NameConstants.SuperAdminRole,
                    NormalizedName = NameConstants.SUPERADMINRole,
                    ConcurrencyStamp = null
                });
            }

            if (!await roleManager.Roles.AnyAsync(r => r.Name == NameConstants.AdminRole))
            {
                await roleManager.CreateAsync(new ApplicationRole()
                {
                    Id = IdConstants.AdminRoleId,
                    Name = NameConstants.AdminRole,
                    NormalizedName = NameConstants.ADMINRole,
                    ConcurrencyStamp = null
                });
            }

            if (!await roleManager.Roles.AnyAsync(r => r.Name == NameConstants.GeneralUserRole))
            {
                await roleManager.CreateAsync(new ApplicationRole()
                {
                    Id = IdConstants.GeneralUserRoleId,
                    Name = NameConstants.GeneralUserRole,
                    NormalizedName = NameConstants.GENERALUSERRole,
                    ConcurrencyStamp = null
                });
            }

            if (!await roleManager.Roles.AnyAsync(r => r.Name == NameConstants.TechnicalRole))
            {
                await roleManager.CreateAsync(new ApplicationRole()
                {
                    Id = IdConstants.TechnicalRoleId,
                    Name = NameConstants.TechnicalRole,
                    NormalizedName = NameConstants.TECHNICALRole,
                    ConcurrencyStamp = null
                });
            }

            if (!await roleManager.Roles.AnyAsync(r => r.Name == NameConstants.DeliveryRole))
            {
                await roleManager.CreateAsync(new ApplicationRole()
                {
                    Id = IdConstants.DeliveryRoleId,
                    Name = NameConstants.DeliveryRole,
                    NormalizedName = NameConstants.DELIVERYRole,
                    ConcurrencyStamp = null
                });
            }

            if (!await roleManager.Roles.AnyAsync(r => r.Name == NameConstants.SupplierRole))
            {
                await roleManager.CreateAsync(new ApplicationRole()
                {
                    Id = IdConstants.SupplierRoleId,
                    Name = NameConstants.SupplierRole,
                    NormalizedName = NameConstants.SUPERADMINRole,
                    ConcurrencyStamp = null
                });
            }
        }
    }
}
