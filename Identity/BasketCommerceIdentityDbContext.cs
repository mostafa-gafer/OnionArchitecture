using Microsoft.EntityFrameworkCore;
using Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Identity
{
    public class BasketCommerceIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string> 
    {
        public BasketCommerceIdentityDbContext(DbContextOptions<BasketCommerceIdentityDbContext> options) : base(options)
        {
        }
    }
    //public class RefrigeratorMaintenanceCenterIdentityDbContext : ApiAuthorizationDbContext<ApplicationUser>
    //{
    //    public RefrigeratorMaintenanceCenterIdentityDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
    //        : base(options, operationalStoreOptions)
    //    {

    //    }
    //}
}
