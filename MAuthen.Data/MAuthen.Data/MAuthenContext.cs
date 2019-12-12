using MAuthen.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MAuthen.Data
{
    public class MAuthenContext : DbContext
    {
        public MAuthenContext(DbContextOptions<MAuthenContext> options) : base(options)
        {
            //base.Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Secret> Secrets { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<UserServiceRoles> UserServiceRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            EnumToNumberConverter<RoleFlags, int>
                converter = new EnumToNumberConverter<RoleFlags, int>();


            modelBuilder.Entity<UserServiceRoles>(entity =>
            {
                entity.HasKey(usr => new { usr.UserId, usr.RoleId, usr.ServiceId });
                entity.HasOne(usr => usr.User)
                    .WithMany(u => u.UserServiceRoles)
                    .HasForeignKey(usr => usr.UserId);
                entity.HasOne(usr => usr.Service)
                    .WithMany(s => s.UserServicesRoles)
                    .HasForeignKey(usr => usr.ServiceId);
                entity.HasOne(usr => usr.Role)
                    .WithMany(r => r.UserServiceRoles)
                    .HasForeignKey(usr => usr.RoleId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(u => u.Secret)
                    .WithOne(s => s.User)
                    .HasForeignKey<Secret>(u => u.IdUser);
                entity.HasMany(c => c.Contacts)
                    .WithOne(u => u.User);
                entity.HasOne(u => u.Secret)
                    .WithOne(s => s.User);
                entity.HasIndex(u => u.UserName).IsUnique();
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasIndex(r => r.Name)
                .IsUnique();
                entity.HasIndex(r => r.Issuer)
                    .IsUnique();
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(r => r.Options)
                .HasConversion(converter);
            });
        }
    }
}
