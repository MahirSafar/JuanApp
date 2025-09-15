using JuanApp.Domain.Common;
using JuanApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JuanApp.Persistance.DAL.Context
{
    public class JuanAppContext(DbContextOptions<JuanAppContext> options) : IdentityDbContext<AppUser, IdentityRole, string>(options)
    {
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Service> Services { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(JuanAppContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries<AuditEntity>();
            var currentTime = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = currentTime;
                        entry.Entity.IsDeleted = false;
                        break;
                    case EntityState.Modified:
                        entry.Property(p => p.Created).IsModified = false;
                        if (entry.Property(p => p.IsDeleted).IsModified &&
                            entry.Property(p => p.IsDeleted).CurrentValue == true)
                        {
                            entry.Entity.Deleted = currentTime;
                        }
                        else
                        {
                            entry.Entity.LastModified = currentTime;
                        }
                        break;
                }
            }
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
