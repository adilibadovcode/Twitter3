using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;
using Twitter.Core.Entities;
using Twitter.Core.Entities.Common;

namespace Twitter.DAL.Contexts
{
    public class TwitterContext : IdentityDbContext<AppUser>
    {
        public TwitterContext(DbContextOptions opt) : base(opt) { }

        public DbSet<Topic> Topics { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.CreatedTime = DateTime.UtcNow;
                //else if(entry.State==EntityState.Modified)
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
