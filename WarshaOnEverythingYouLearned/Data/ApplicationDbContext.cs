using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WarshaOnEverythingYouLearned.Configrations;
using WarshaOnEverythingYouLearned.Data.Entity;

namespace WarshaOnEverythingYouLearned.Data
{
    public class ApplicationDbContext:DbContext
    {
        public IOptions< DbOptions> dbOptions;//appsettings.json attriputes
        public DbSet<User> Users { get; set; }
        public DbSet<Role>roles { get; set; }
        public DbSet<Permission>permissions { get; set; }
        public DbSet<RolePermission>rolesPermission { get; set; }



        public ApplicationDbContext(IOptions<DbOptions> dbOptions)
        {
            this.dbOptions = dbOptions;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var options = dbOptions.Value;
            optionsBuilder.UseSqlServer(options.DefaultLink);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolePermission>()
      .HasKey(rp => new { rp.RoleId, rp.PermissionId });

        }
    }
}
