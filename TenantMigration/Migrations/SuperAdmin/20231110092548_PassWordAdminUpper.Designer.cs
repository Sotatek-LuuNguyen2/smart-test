﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PostgreDataContext;

#nullable disable

namespace TenantMigration.Migrations.SuperAdmin
{
    [DbContext(typeof(SuperAdminContext))]
    [Migration("20231110092548_PassWordAdminUpper")]
    partial class PassWordAdminUpper
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Entity.SuperAdmin.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CREATE_DATE");

                    b.Property<string>("FullName")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("FULL_NAME");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("integer")
                        .HasColumnName("IS_DELETED");

                    b.Property<int>("LoginId")
                        .HasColumnType("integer")
                        .HasColumnName("LOGIN_ID");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("NAME");

                    b.Property<string>("PassWord")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("PASSWORD");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("ROLE");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UPDATE_DATE");

                    b.HasKey("Id");

                    b.HasIndex("LoginId")
                        .IsUnique();

                    b.ToTable("ADMIN");
                });

            modelBuilder.Entity("Entity.SuperAdmin.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CREATE_DATE");

                    b.Property<string>("ENDPOINTDB")
                        .HasColumnType("text")
                        .HasColumnName("MESSAGE");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("integer")
                        .HasColumnName("IS_DELETED");

                    b.Property<byte>("Status")
                        .HasColumnType("smallint")
                        .HasColumnName("STATUS");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UPDATE_DATE");

                    b.HasKey("Id");

                    b.ToTable("NOTIFICATION");
                });

            modelBuilder.Entity("Entity.SuperAdmin.Scription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CREATE_DATE");

                    b.Property<string>("HOSPITAL")
                        .HasColumnType("text")
                        .HasColumnName("HOSPITAL");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("integer")
                        .HasColumnName("IS_DELETED");

                    b.Property<string>("ScriptString")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ScriptString");

                    b.Property<int>("TenantId")
                        .HasColumnType("integer")
                        .HasColumnName("TENANT_ID");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UPDATE_DATE");

                    b.HasKey("Id");

                    b.ToTable("SCRIPTION");
                });

            modelBuilder.Entity("Entity.SuperAdmin.Tenant", b =>
                {
                    b.Property<int>("TenantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("TENANT_ID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TenantId"));

                    b.Property<int>("Action")
                        .HasColumnType("integer")
                        .HasColumnName("ACTION");

                    b.Property<int>("AdminId")
                        .HasColumnType("integer")
                        .HasColumnName("ADMIN_ID");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CREATE_DATE");

                    b.Property<string>("Db")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("DB");

                    b.Property<string>("EndPointDb")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("END_POINT_DB");

                    b.Property<string>("EndSubDomain")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("END_SUB_DOMAIN");

                    b.Property<string>("Hospital")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("HOSPITAL");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("integer")
                        .HasColumnName("IS_DELETED");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("PASSWORD");

                    b.Property<int>("ScheduleDate")
                        .HasColumnType("integer")
                        .HasColumnName("SCHEDULE_DATE");

                    b.Property<int>("ScheduleTime")
                        .HasColumnType("integer")
                        .HasColumnName("SCHEDULE_TIME");

                    b.Property<int>("Size")
                        .HasColumnType("integer")
                        .HasColumnName("SIZE");

                    b.Property<byte>("Status")
                        .HasColumnType("smallint")
                        .HasColumnName("STATUS");

                    b.Property<string>("SubDomain")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("SUB_DOMAIN");

                    b.Property<byte>("Type")
                        .HasColumnType("smallint")
                        .HasColumnName("TYPE");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UPDATE_DATE");

                    b.HasKey("TenantId");

                    b.HasIndex("AdminId")
                        .IsUnique();

                    b.HasIndex("Db")
                        .IsUnique();

                    b.HasIndex("EndPointDb")
                        .IsUnique();

                    b.HasIndex("EndSubDomain")
                        .IsUnique();

                    b.HasIndex("Hospital")
                        .IsUnique();

                    b.HasIndex("SubDomain")
                        .IsUnique();

                    b.ToTable("TENANT");
                });
#pragma warning restore 612, 618
        }
    }
}
