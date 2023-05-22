using System;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Authorization
{
    public class InjectPolices
    {
        public static void AddPolicies(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {

                #region Staff.

                options.AddPolicy(Authorization.Policies.ListStaffs, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.StaffsList));
                options.AddPolicy(Authorization.Policies.CreateStaff, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.StaffCreate));
                options.AddPolicy(Authorization.Policies.ViewStaff, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.StaffView));
                options.AddPolicy(Authorization.Policies.EditStaff, policy => policy.RequireClaim(ClaimConstants.Permission, new string[] { ApplicationPermissions.StaffEdit }));
                options.AddPolicy(Authorization.Policies.DeleteStaff, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.StaffDelete));

                #endregion

                #region Order.

                options.AddPolicy(Authorization.Policies.ListOrders, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.OrdersList));
                options.AddPolicy(Authorization.Policies.CreateOrder, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.OrderCreate));
                options.AddPolicy(Authorization.Policies.ViewOrder, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.OrderView));
                options.AddPolicy(Authorization.Policies.EditOrder, policy => policy.RequireClaim(ClaimConstants.Permission, new string[] { ApplicationPermissions.OrderEdit }));
                options.AddPolicy(Authorization.Policies.DeleteOrder, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.OrderDelete));

                #endregion

                #region Admin Manage Roles.

                options.AddPolicy(Authorization.Policies.ListRoles, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.RolesList));
                options.AddPolicy(Authorization.Policies.EditRole, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.RoleEdit));
                options.AddPolicy(Authorization.Policies.CreateRole, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.RoleCreate));
                options.AddPolicy(Authorization.Policies.DeleteRole, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.RoleDelete));
                options.AddPolicy(Authorization.Policies.ViewRole, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.RoleView));
                #endregion

                #region Admin Manage Users.

                options.AddPolicy(Authorization.Policies.ListUsers, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.UsersList));
                options.AddPolicy(Authorization.Policies.EditUser, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.UserEdit));
                options.AddPolicy(Authorization.Policies.CreateUser, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.UserCreate));
                options.AddPolicy(Authorization.Policies.DeleteUser, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.UserDelete));
                options.AddPolicy(Authorization.Policies.ViewUser, policy => policy.RequireClaim(ClaimConstants.Permission, ApplicationPermissions.UserView));

                #endregion

            });
        }
    }
}
