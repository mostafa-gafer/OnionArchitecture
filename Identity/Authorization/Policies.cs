
namespace Identity.Authorization
{
    public class Policies
    {
        #region Policy.

        ///<summary>Policy to allow viewing all user records.</summary>
        public const string ViewAllUsersPolicy = "View All Users";

        ///<summary>Policy to allow adding, removing and updating all user records.</summary>
        public const string ManageAllUsersPolicy = "Manage All Users";

        /// <summary>Policy to allow viewing details of all roles.</summary>
        public const string ViewAllRolesPolicy = "View All Roles";

        /// <summary>Policy to allow viewing details of all or specific roles (Requires roleName as parameter).</summary>
        public const string ViewRoleByRoleNamePolicy = "View Role by RoleName";

        /// <summary>Policy to allow adding, removing and updating all roles.</summary>
        public const string ManageAllRolesPolicy = "Manage All Roles";

        /// <summary>Policy to allow assigning roles the user has access to (Requires new and current roles as parameter).</summary>
        public const string AssignAllowedRolesPolicy = "Assign Allowed Roles";

        /// <summary>Policy to allow assigning roles the user has access to (Requires new and current roles as parameter).</summary>

        // Permissions
        public const string ViewSystemDropDownLists = "System DropDown Lists";

        #endregion

        #region Staff.

        //---- Staff
        public const string ListStaffs = "List Staffs";
        public const string EditStaff = "Edit Staff";
        public const string CreateStaff = "Create Staff";
        public const string ViewStaff = "View Staff";
        public const string DeleteStaff = "Delete Staff";

        #endregion

        #region Order.

        //---- Staff
        public const string ListOrders = "List Orders";
        public const string EditOrder = "Edit Order";
        public const string CreateOrder = "Create Order";
        public const string ViewOrder = "View Order";
        public const string DeleteOrder = "Delete Order";

        #endregion

        #region Admin Manage Roles And Admin Manage Users.

        //---- Admin Manage Roles And Admin Manage Users.
        public const string ListRoles = "List Roles";
        public const string EditRole = "Edit Role";
        public const string CreateRole = "Create Role";
        public const string DeleteRole = "Delete Role";
        public const string ViewRole = "View Role";

        //---- Admin Manage Users.
        public const string ListUsers = "List User";
        public const string EditUser = "Edit User";
        public const string CreateUser = "Create User";
        public const string DeleteUser = "Delete User";
        public const string ViewUser = "View User";

        #endregion

    }

    /// <summary>
    /// Operation Policy to allow adding, viewing, updating and deleting general or specific user records.
    /// </summary>
    public static class AccountManagementOperations
    {
        public const string CreateOperationName = "Create";
        public const string ReadOperationName = "Read";
        public const string UpdateOperationName = "Update";
        public const string DeleteOperationName = "Delete";

        public static UserAccountAuthorizationRequirement Create = new UserAccountAuthorizationRequirement(CreateOperationName);
        public static UserAccountAuthorizationRequirement Read = new UserAccountAuthorizationRequirement(ReadOperationName);
        public static UserAccountAuthorizationRequirement Update = new UserAccountAuthorizationRequirement(UpdateOperationName);
        public static UserAccountAuthorizationRequirement Delete = new UserAccountAuthorizationRequirement(DeleteOperationName);
    }
}
