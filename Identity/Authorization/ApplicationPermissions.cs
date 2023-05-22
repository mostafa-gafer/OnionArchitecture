
using System.Collections.ObjectModel;

namespace Identity.Authorization
{
    public static class ApplicationPermissions
    {
        public static ReadOnlyCollection<ApplicationPermission> AllPermissions;
        
        #region Staff.

        public const string StaffGroupName = "Staff";
        public static ApplicationPermission StaffsList = new ApplicationPermission("Staffs List", "staffs.list", StaffGroupName, "Staff GroupName");
        public static ApplicationPermission StaffEdit = new ApplicationPermission("Staff Edit", "staff.edit", StaffGroupName, "Staff GroupName");
        public static ApplicationPermission StaffCreate = new ApplicationPermission("Staff Create", "staff.create", StaffGroupName, "Staff GroupName");
        public static ApplicationPermission StaffDelete = new ApplicationPermission("Staff Delete", "staff.delete", StaffGroupName, "Staff GroupName");
        public static ApplicationPermission StaffView = new ApplicationPermission("Staff View", "staff.view", StaffGroupName, " Staff GroupName");

        #endregion

        #region Order.

        public const string OrderGroupName = "Order";
        public static ApplicationPermission OrdersList = new ApplicationPermission("Orders List", "orders.list", OrderGroupName, "Order GroupName");
        public static ApplicationPermission OrderEdit = new ApplicationPermission("Order Edit", "order.edit", OrderGroupName, "Order GroupName");
        public static ApplicationPermission OrderCreate = new ApplicationPermission("Order Create", "order.create", OrderGroupName, "Order GroupName");
        public static ApplicationPermission OrderDelete = new ApplicationPermission("Order Delete", "order.delete", OrderGroupName, "Order GroupName");
        public static ApplicationPermission OrderView = new ApplicationPermission("Order View", "order.view", OrderGroupName, " Order GroupName");

        #endregion

        #region Roles.

        //---- Roles
        public const string RoleGroupName = "Role";
        public static ApplicationPermission RolesList = new ApplicationPermission("Roles List", "roles.list", RoleGroupName, "Role GroupName");
        public static ApplicationPermission RoleEdit = new ApplicationPermission("Role Edit", "role.edit", RoleGroupName, " Role GroupName");
        public static ApplicationPermission RoleCreate = new ApplicationPermission("Role Create", "role.create", RoleGroupName, "Role GroupName");
        public static ApplicationPermission RoleDelete = new ApplicationPermission("Role Delete", "role.delete", RoleGroupName, "Role GroupName");
        public static ApplicationPermission RoleView = new ApplicationPermission("Role View", "role.view", RoleGroupName, "Role GroupName");

        #endregion

        #region Users.

        //---- Users.
        public const string UserGroupName = "User";
        public static ApplicationPermission UsersList = new ApplicationPermission("Users List", "users.list", UserGroupName, "User GroupName");
        public static ApplicationPermission UserEdit = new ApplicationPermission("User Edit", "user.edit", UserGroupName, "User GroupName");
        public static ApplicationPermission UserCreate = new ApplicationPermission("User Create", "user.create", UserGroupName, "User GroupName");
        public static ApplicationPermission UserDelete = new ApplicationPermission("User Delete", "user.delete", UserGroupName, "User GroupName");
        public static ApplicationPermission UserView = new ApplicationPermission("User View", "user.view", UserGroupName, "User GroupName");

        #endregion

        static ApplicationPermissions()
        {
            List<ApplicationPermission> allPermissions = new List<ApplicationPermission>()
            {
               #region Staff.

                StaffsList ,
                StaffEdit ,
                StaffCreate,
                StaffDelete,
                StaffView ,
                
                #endregion

                #region Order.

                OrdersList ,
                OrderEdit ,
                OrderCreate,
                OrderDelete,
                OrderView ,
                
                #endregion

                #region Roles.

                RolesList,
                RoleEdit,
                RoleCreate,
                RoleDelete,
                RoleView,

                #endregion

                #region User.

                UsersList,
                UserEdit,
                UserCreate,
                UserDelete,
                UserView,

                #endregion

            };

            AllPermissions = allPermissions.AsReadOnly();
        }

        public static ApplicationPermission GetPermissionByName(string permissionName)
        {
            return AllPermissions.Where(p => p.Name == permissionName).SingleOrDefault();
        }

        public static ApplicationPermission GetPermissionByValue(string permissionValue)
        {
            return AllPermissions.Where(p => p.Value == permissionValue).FirstOrDefault();
        }

        public static List<ApplicationPermission> GetPermissionsByValue(IEnumerable<string> permissionsValue)
        {
            return AllPermissions.Where(p => permissionsValue.Contains(p.Value)).ToList();
        }

        public static string[] GetAllPermissionValues()
        {
            return AllPermissions.Select(p => p.Value).ToArray();
        }

        
    }

    public class ApplicationPermission
    {
        public ApplicationPermission()
        { }

        public ApplicationPermission(string name, string value, string groupName, string description = null)
        {
            Name = name;
            Value = value;
            GroupName = groupName;
            Description = description;
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return Value;
        }


        public static implicit operator string(ApplicationPermission permission)
        {
            return permission.Value;
        }
    }
}
