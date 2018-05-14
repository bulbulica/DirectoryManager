﻿// <auto-generated />
using System;
using IdentityServer.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IdentityServer.Persistence.EF.Migrations
{
    [DbContext(typeof(EmployeeDbContext))]
    partial class EmployeeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rc1-32029")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IdentityServer.Domain.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DepartmentManagerId");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentManagerId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("IdentityServer.Domain.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("CV");

                    b.Property<int?>("DepartmentId");

                    b.Property<int?>("DepartmentId1");

                    b.Property<string>("Name");

                    b.Property<string>("Picture");

                    b.Property<int>("PositionId");

                    b.Property<int?>("TeamId");

                    b.Property<int?>("TeamId1");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("DepartmentId1");

                    b.HasIndex("PositionId");

                    b.HasIndex("TeamId");

                    b.HasIndex("TeamId1");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("IdentityServer.Domain.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessLevel");

                    b.Property<string>("Description");

                    b.Property<string>("RoleName");

                    b.HasKey("Id");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("IdentityServer.Domain.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("DepartmentId");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("TeamLeaderId");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("TeamLeaderId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("IdentityServer.Domain.Department", b =>
                {
                    b.HasOne("IdentityServer.Domain.Employee", "DepartmentManager")
                        .WithMany()
                        .HasForeignKey("DepartmentManagerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("IdentityServer.Domain.Employee", b =>
                {
                    b.HasOne("IdentityServer.Domain.Department")
                        .WithMany("Employees")
                        .HasForeignKey("DepartmentId");

                    b.HasOne("IdentityServer.Domain.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId1");

                    b.HasOne("IdentityServer.Domain.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("IdentityServer.Domain.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");

                    b.HasOne("IdentityServer.Domain.Team")
                        .WithMany("Employees")
                        .HasForeignKey("TeamId1");
                });

            modelBuilder.Entity("IdentityServer.Domain.Team", b =>
                {
                    b.HasOne("IdentityServer.Domain.Department", "Department")
                        .WithMany("Teams")
                        .HasForeignKey("DepartmentId");

                    b.HasOne("IdentityServer.Domain.Employee", "TeamLeader")
                        .WithMany()
                        .HasForeignKey("TeamLeaderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
