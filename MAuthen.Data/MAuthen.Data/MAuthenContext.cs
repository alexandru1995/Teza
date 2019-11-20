﻿using MAuthen.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
        //public DbSet<UserService> UserServices { get; set; }
        //public DbSet<ServiceRole> ServiceRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS;Initial Catalog=MAuthen;Integrated Security=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            //modelBuilder.Entity<UserRole>(entity =>
            //{
            //    entity.HasKey(ur => new { ur.IdRole, ur.IdUser });
            //    entity.HasOne(ur => ur.User)
            //        .WithMany(u => u.UserRoles)
            //        .HasForeignKey(ur => ur.IdUser);
            //    entity.HasOne(ur => ur.Role)
            //        .WithMany(r => r.UserRoles)
            //        .HasForeignKey(ur => ur.IdRole);

            //});

            //modelBuilder.Entity<UserService>(entyty =>
            //{
            //    entyty.HasKey(us => new { us.IdUser, us.IdService });
            //    entyty.HasOne(us => us.User)
            //        .WithMany(u => u.UserServices)
            //        .HasForeignKey(ur => ur.IdUser);
            //    entyty.HasOne(us => us.Service)
            //        .WithMany(s => s.UserServices)
            //        .HasForeignKey(us => us.IdService);
            //});

            //modelBuilder.Entity<ServiceRole>(entity =>
            //{
            //    entity.HasKey(sr => new
            //    {
            //        sr.IdRole,
            //        sr.IdService
            //    });
            //    entity.HasOne(sr => sr.Role)
            //        .WithMany(u => u.ServiceRoles)
            //        .HasForeignKey(ur => ur.IdRole);
            //    entity.HasOne(sr => sr.Service)
            //        .WithMany(s => s.ServiceRoles)
            //        .HasForeignKey(us => us.IdService);
            //});

            //modelBuilder.Entity<Contacts>(entity =>
            //{
            //    entity.HasMany(e => e.Email)
            //        .WithOne(e => e.Contact);
            //    entity.HasMany(e => e.Phone)
            //        .WithOne(e => e.Contact);
            //});

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
                entity.HasIndex(r => r.Domain)
                    .IsUnique();
            });
        }
    }
}
