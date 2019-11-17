﻿// <auto-generated />
using System;
using MAuthen.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MAuthen.Data.Migrations
{
    [DbContext(typeof(MAuthenContext))]
    [Migration("20191117162522_addRefreshToken")]
    partial class addRefreshToken
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MAuthen.Domain.Entities.Contacts", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Phone");

                    b.Property<Guid?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("Phone")
                        .IsUnique()
                        .HasFilter("[Phone] IS NOT NULL");

                    b.HasIndex("UserId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("MAuthen.Domain.Models.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("MAuthen.Domain.Models.Secret", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("IdUser");

                    b.Property<string>("Password");

                    b.Property<Guid>("RefreshToken");

                    b.Property<string>("TotpSecret");

                    b.HasKey("Id");

                    b.HasIndex("IdUser")
                        .IsUnique();

                    b.ToTable("Secrets");
                });

            modelBuilder.Entity("MAuthen.Domain.Models.Service", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Certificate");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("HasTotp");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("MAuthen.Domain.Models.ServiceRole", b =>
                {
                    b.Property<Guid>("IdRole");

                    b.Property<Guid>("IdService");

                    b.HasKey("IdRole", "IdService");

                    b.HasIndex("IdService");

                    b.ToTable("ServiceRoles");
                });

            modelBuilder.Entity("MAuthen.Domain.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Birthday");

                    b.Property<bool>("Blocked");

                    b.Property<DateTime>("BlockedTime");

                    b.Property<string>("FirstName");

                    b.Property<bool>("Gender");

                    b.Property<string>("LastName");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasFilter("[UserName] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MAuthen.Domain.Models.UserRole", b =>
                {
                    b.Property<Guid>("IdRole");

                    b.Property<Guid>("IdUser");

                    b.Property<DateTime>("CreatedOn");

                    b.HasKey("IdRole", "IdUser");

                    b.HasIndex("IdUser");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("MAuthen.Domain.Models.UserService", b =>
                {
                    b.Property<Guid>("IdUser");

                    b.Property<Guid>("IdService");

                    b.Property<bool>("Consent");

                    b.Property<DateTime>("CreatedOn");

                    b.HasKey("IdUser", "IdService");

                    b.HasIndex("IdService");

                    b.ToTable("UserServices");
                });

            modelBuilder.Entity("MAuthen.Domain.Entities.Contacts", b =>
                {
                    b.HasOne("MAuthen.Domain.Models.User")
                        .WithMany("Contacts")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("MAuthen.Domain.Models.Secret", b =>
                {
                    b.HasOne("MAuthen.Domain.Models.User", "User")
                        .WithOne("Secret")
                        .HasForeignKey("MAuthen.Domain.Models.Secret", "IdUser")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MAuthen.Domain.Models.ServiceRole", b =>
                {
                    b.HasOne("MAuthen.Domain.Models.Role", "Role")
                        .WithMany("ServiceRoles")
                        .HasForeignKey("IdRole")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MAuthen.Domain.Models.Service", "Service")
                        .WithMany("ServiceRoles")
                        .HasForeignKey("IdService")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MAuthen.Domain.Models.UserRole", b =>
                {
                    b.HasOne("MAuthen.Domain.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("IdRole")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MAuthen.Domain.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MAuthen.Domain.Models.UserService", b =>
                {
                    b.HasOne("MAuthen.Domain.Models.Service", "Service")
                        .WithMany("UserServices")
                        .HasForeignKey("IdService")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MAuthen.Domain.Models.User", "User")
                        .WithMany("UserServices")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
