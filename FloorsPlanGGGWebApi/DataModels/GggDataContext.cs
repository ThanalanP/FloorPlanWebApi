using System.Data.Entity;

namespace FloorsPlanGGGWebApi.DataModels
{
    public class GggDataContext : DbContext
    {
        public GggDataContext()
            : base("name=GggDataContext")
        {
        }

        public virtual DbSet<Floor> Floors { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Floor>()
               .Property(e => e.Id)
               .IsRequired();

            modelBuilder.Entity<Floor>()
                .Property(e => e.DisplayName)
                .IsFixedLength();

            modelBuilder.Entity<Floor>()
                .HasMany(e => e.Rooms)
                .WithRequired(e => e.Floor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Room>()
               .Property(e => e.Id)
               .IsRequired();

            modelBuilder.Entity<Room>()
                .Property(e => e.DisplayName)
                .IsFixedLength();

            modelBuilder.Entity<Room>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Room)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Id)
                .IsRequired();
        }
    }
}
