using Microsoft.EntityFrameworkCore;

namespace Tohi.Client.Signalr.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserEntities>().ToTable(tb => tb.HasTrigger("TriggerUser")).HasIndex(u => u.Id);

            builder.Entity<FollowEntities>().ToTable(tb => tb.HasTrigger("TriggerFollow")).HasIndex(u => u.Id);


            builder.Entity<CdnLiveEntities>().ToTable(tb => tb.HasTrigger("TriggerCdn")).HasIndex(u => u.Id);

            builder.Entity<StreamEntities>().ToTable(tb => tb.HasTrigger(typeof(StreamEntities).Name)).HasIndex(u => u.Id);


            builder.Entity<MessageEntities>().ToTable(tb => tb.HasTrigger("TriggerMessage")).HasIndex(u => u.Id);
        }

        public virtual DbSet<UserEntities> UserEntities { get; set; }
        public virtual DbSet<FollowEntities> FollowEntities { get; set; }

        public virtual DbSet<CdnLiveEntities> CdnLiveEntities { get; set; }
        public virtual DbSet<StreamEntities> StreamEntities { get; set; }

        public virtual DbSet<MessageEntities> MessageEntities { get; set; }
    }
}
