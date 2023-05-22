using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Authorization
{
    public static class ClaimConstants
    {
        ///<summary>A claim that specifies the subject of an entity</summary>
        public const string Subject = "sub";

        ///<summary>A claim that specifies the permission of an entity</summary>
        public const string Permission = "permission";

        //Staff Claims 
        public const string Staffslist = "staffs.list";
        public const string StaffEdit = "staff.edit";
        public const string StaffCreate = "staff.create";
        public const string StaffDelete = "staff.delete";
        public const string StaffView = "staff.view";

        //Order Claims 
        public const string Orderslist = "orders.list";
        public const string OrderEdit = "order.edit";
        public const string OrderCreate = "order.create";
        public const string OrderDelete = "order.delete";
        public const string OrderView = "order.view";

        //Role Claims 
        public const string Roleslist = "roles.list";
        public const string RoleEdit = "role.edit";
        public const string RoleCreate = "role.create";
        public const string RoleDelete = "role.delete";
        public const string RoleView = "role.view";

        //User Claims 
        public const string Userslist = "users.list";
        public const string UserEdit = "user.edit";
        public const string UserCreate = "user.create";
        public const string UserDelete = "user.delete";
        public const string UserView = "user.view";

        public const string StaffsMenu = "menu.staffs";
        public const string OrdersMenu = "menu.orders";
        public const string RolesMenu = "menu.roles";
        public const string UsersMenu = "menu.users";
    }

    public static class IdConstants
    {
        // AspNet Ids
        public const string SuperAdminId = "043dc34c-10a8-471e-becf-d638ff40c819";
        public const string AdminId = "143ac34c-10a8-471e-becf-d638ff40c819";
        public const string TechnicalId = "153ac44c-10a8-471e-becf-d638ff40c889";

        public const string SuperAdminRoleId = "de046313-9646-4a44-9a01-cc1dbd649eo8";
        public const string AdminRoleId = "ei1684ef71-2f48-45e1-b9fc-b157842220527";
        public const string GeneralUserRoleId = "r1584ef71-2f48-45e1-b9fc-b157842220527";
        public const string TechnicalRoleId = "t1584ef70-2f49-45e1-b9fc-b157842220527";
        public const string DeliveryRoleId = "d1584ef72-2f49-45e1-b9fc-b157842220527";
        public const string SupplierRoleId = "s1584ef73-2f49-45e1-b9fc-b157842220527";

    }

    public static class NameConstants
    {
        // AspNet Role Names
        public const string SuperAdminRole = "Super Admin";
        public const string SUPERADMINRole = "SUPER ADMIN";

        public const string AdminRole = "Admin";
        public const string ADMINRole = "ADMIN";

        public const string GeneralUserRole = "General User";
        public const string GENERALUSERRole = "GENERAL USER";

        public const string TechnicalRole = "Technical";
        public const string TECHNICALRole = "TECHNICAL";

        public const string DeliveryRole = "Delivery";
        public const string DELIVERYRole = "DELIVERY";

        public const string SupplierRole = "Supplier";
        public const string SUPPLIERRole = "SUPPLIER";

    }

    public static class EmailConstants
    {
        // AspNet User Emails
        public const string SuperAdminEmail = "mostafa.fathy85.mf@gmail.com";
        public const string AdminEmail = "hassan.gafer@gmail.com";
        public const string TechnicalEmail = "karem.gafer@gmail.com";

    }
}
