﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volo.Abp.EntityFrameworkCore;
using Washyn.UNAJ.Lot.Data;

#nullable disable

namespace Washyn.UNAJ.Lot.Migrations
{
    [DbContext(typeof(LotDbContext))]
    partial class LotDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("_Abp_DatabaseProvider", EfCoreDatabaseProvider.Sqlite)
                .HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("Acme.BookStore.Entities.AppSettings", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.ToTable("AppSettings");
                });

            modelBuilder.Entity("Acme.BookStore.Entities.Comision", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Comisions");
                });

            modelBuilder.Entity("Acme.BookStore.Entities.Curso", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Cursos");
                });

            modelBuilder.Entity("Acme.BookStore.Entities.Docente", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ApellidoMaterno")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ApellidoPaterno")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("Area")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT")
                        .HasColumnName("CreationTime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("TEXT")
                        .HasColumnName("CreatorId");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("TEXT")
                        .HasColumnName("DeleterId");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("TEXT")
                        .HasColumnName("DeletionTime");

                    b.Property<string>("Dni")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Genero")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("GradoId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false)
                        .HasColumnName("IsDeleted");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("TEXT")
                        .HasColumnName("LastModificationTime");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("TEXT")
                        .HasColumnName("LastModifierId");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GradoId");

                    b.ToTable("Docentes");
                });

            modelBuilder.Entity("Acme.BookStore.Entities.Grado", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Prefix")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Grados");
                });

            modelBuilder.Entity("Acme.BookStore.Entities.Participante", b =>
                {
                    b.Property<Guid>("ComisionId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("DocenteId")
                        .HasColumnType("TEXT");

                    b.HasKey("ComisionId", "DocenteId");

                    b.ToTable("Participantes");
                });

            modelBuilder.Entity("Acme.BookStore.Entities.Rol", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ComisionId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ComisionId");

                    b.ToTable("Rols");
                });

            modelBuilder.Entity("Acme.BookStore.Entities.Sorteo", b =>
                {
                    b.Property<Guid>("DocenteId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RolId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ComisionId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT")
                        .HasColumnName("CreationTime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("TEXT")
                        .HasColumnName("CreatorId");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("TEXT")
                        .HasColumnName("DeletionTime");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false)
                        .HasColumnName("IsDeleted");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("TEXT")
                        .HasColumnName("LastModificationTime");

                    b.HasKey("DocenteId", "RolId", "ComisionId");

                    b.ToTable("Sorteo");
                });

            modelBuilder.Entity("Acme.BookStore.Entities.Docente", b =>
                {
                    b.HasOne("Acme.BookStore.Entities.Grado", "Grado")
                        .WithMany()
                        .HasForeignKey("GradoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Grado");
                });

            modelBuilder.Entity("Acme.BookStore.Entities.Rol", b =>
                {
                    b.HasOne("Acme.BookStore.Entities.Comision", null)
                        .WithMany("Rols")
                        .HasForeignKey("ComisionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Acme.BookStore.Entities.Comision", b =>
                {
                    b.Navigation("Rols");
                });
#pragma warning restore 612, 618
        }
    }
}
