using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Identity.Authorization;

namespace Identity.Seed
{
    public static class CreateRoleClaims
    {
        public static async Task SeedAsync(BasketCommerceIdentityDbContext _context)
        {
            //Super Admin Role Claims
            if (!await _context.RoleClaims.AnyAsync(c => c.RoleId == IdConstants.SuperAdminRoleId))
            {
                
                List<IdentityRoleClaim<string>> superAdminRoleClaims = new List<IdentityRoleClaim<string>>() {
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.Staffslist,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.StaffEdit,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.StaffCreate,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.StaffDelete,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.StaffView,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.Orderslist,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrderEdit,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrderCreate,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrderDelete,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrderView,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.Roleslist,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.RoleEdit,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.RoleCreate,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.RoleDelete,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.RoleView,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.Userslist,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.UserEdit,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.UserCreate,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.UserDelete,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.UserView,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.StaffsMenu,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrdersMenu,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.RolesMenu,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SuperAdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.UsersMenu,
                },
            };
                await _context.RoleClaims.AddRangeAsync(superAdminRoleClaims);
            }

            //Admin Role Claims
            if (!await _context.RoleClaims.AnyAsync(c => c.RoleId == IdConstants.AdminRoleId))
            {
                List<IdentityRoleClaim<string>> adminRoleClaims = new List<IdentityRoleClaim<string>>() {
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.Staffslist,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.StaffEdit,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.StaffCreate,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.StaffDelete,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.StaffView,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.Orderslist,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrderEdit,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrderCreate,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrderDelete,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrderView,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.Roleslist,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.RoleEdit,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.RoleCreate,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.RoleDelete,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.RoleView,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.Userslist,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.UserEdit,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.UserCreate,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.UserDelete,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.UserView,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.StaffsMenu,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrdersMenu,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.RolesMenu,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.AdminRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.UsersMenu,
                },
            };
                await _context.RoleClaims.AddRangeAsync(adminRoleClaims);
            }

            //Generl User Role Claims
            if (!await _context.RoleClaims.AnyAsync(c => c.RoleId == IdConstants.GeneralUserRoleId))
            {
                List<IdentityRoleClaim<string>> generlUserRoleClaims = new List<IdentityRoleClaim<string>>() {
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.GeneralUserRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.Orderslist,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.GeneralUserRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrderView,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.GeneralUserRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrdersMenu,
                },
                
            };
                await _context.RoleClaims.AddRangeAsync(generlUserRoleClaims);
            }

            //Technical Role Claims
            if (!await _context.RoleClaims.AnyAsync(c => c.RoleId == IdConstants.TechnicalRoleId))
            {
                List<IdentityRoleClaim<string>> technicalRoleClaims = new List<IdentityRoleClaim<string>>() {
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.TechnicalRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.Orderslist,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.TechnicalRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrderEdit,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.TechnicalRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrderView,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.TechnicalRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrdersMenu,
                },
            };
                await _context.RoleClaims.AddRangeAsync(technicalRoleClaims);
            }

            //Delivery Role Claims
            if (!await _context.RoleClaims.AnyAsync(c => c.RoleId == IdConstants.DeliveryRoleId))
            {
                List<IdentityRoleClaim<string>> deliveryRoleClaims = new List<IdentityRoleClaim<string>>() {
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.DeliveryRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.Orderslist,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.TechnicalRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrderEdit,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.DeliveryRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrderView,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.DeliveryRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrdersMenu,
                },
            };
                await _context.RoleClaims.AddRangeAsync(deliveryRoleClaims);
            }

            //Supplier Role Claims
            if (!await _context.RoleClaims.AnyAsync(c => c.RoleId == IdConstants.SupplierRoleId))
            {
                
                List<IdentityRoleClaim<string>> supplierRoleClaims = new List<IdentityRoleClaim<string>>() {
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SupplierRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.Orderslist,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SupplierRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrderEdit,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SupplierRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrderView,
                },
                new IdentityRoleClaim<string>
                {
                    RoleId = IdConstants.SupplierRoleId,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = ClaimConstants.OrdersMenu,
                },
            };

                await _context.RoleClaims.AddRangeAsync(supplierRoleClaims);
            }
            
            _context.SaveChanges();
        }
    }
}
