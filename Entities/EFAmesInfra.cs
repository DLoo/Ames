namespace Ames.Entities {
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    public partial class EFAmesInfra : DbContext {
        public EFAmesInfra()
            : base("name=EFAmesInfra") {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<EFAmesInfra, Ames.Migrations.Configuration>("EFAmesInfra"));
            
        }

        public virtual DbSet<ProfileActionLog> ProfileActionLogs { get; set; }

        public virtual DbSet<EFileInfo> EFileInfo { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<ProfileActionLog>()
                .Property(e => e.ActionDuration)
                .HasPrecision(14, 4);

            modelBuilder.Entity<ProfileActionLog>()
                .Property(e => e.ResultDuration)
                .HasPrecision(14, 4);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
