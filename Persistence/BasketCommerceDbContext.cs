using Microsoft.EntityFrameworkCore;
using Application.Contracts;
using Domain.Common;

namespace Persistence
{
    public class BasketCommerceDbContext : DbContext
    {
        private readonly ILoggedInUserService _loggedInUserService;

        public BasketCommerceDbContext(DbContextOptions<BasketCommerceDbContext> options)
           : base(options)
        {
        }

        public BasketCommerceDbContext(DbContextOptions<BasketCommerceDbContext> options, ILoggedInUserService loggedInUserService)
            : base(options)
        {
            _loggedInUserService = loggedInUserService;
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BasketCommerceDbContext).Assembly);

            //seed data, added through migrations

           
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = _loggedInUserService.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = _loggedInUserService.UserId;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
