﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SalonYonetimUygulamasi.Models;

#nullable disable

namespace SalonYonetimUygulamasi.Migrations
{
    [DbContext(typeof(SalonContext))]
    partial class SalonContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SalonYonetimUygulamasi.Models.Calisan", b =>
                {
                    b.Property<int>("CalisanID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CalisanID"));

                    b.Property<string>("CalisanAd")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CalisanSoyad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SalonID")
                        .HasColumnType("int");

                    b.Property<string>("UzmanlikAlani")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CalisanID");

                    b.HasIndex("SalonID");

                    b.ToTable("Calisanlar");
                });

            modelBuilder.Entity("SalonYonetimUygulamasi.Models.Randevu", b =>
                {
                    b.Property<int>("RandevuID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RandevuID"));

                    b.Property<int>("CalisanID")
                        .HasColumnType("int");

                    b.Property<int>("CalısanID")
                        .HasColumnType("int");

                    b.Property<string>("Islem")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Tarih")
                        .HasColumnType("datetime2");

                    b.Property<double>("Ucret")
                        .HasColumnType("float");

                    b.HasKey("RandevuID");

                    b.HasIndex("CalisanID");

                    b.ToTable("Randevular");
                });

            modelBuilder.Entity("SalonYonetimUygulamasi.Models.Salon", b =>
                {
                    b.Property<int>("SalonID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SalonID"));

                    b.Property<string>("SalonAd")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SalonAdres")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SalonTelefon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SalonID");

                    b.ToTable("Salonlar");
                });

            modelBuilder.Entity("SalonYonetimUygulamasi.Models.Calisan", b =>
                {
                    b.HasOne("SalonYonetimUygulamasi.Models.Salon", "Salon")
                        .WithMany("Calisanlar")
                        .HasForeignKey("SalonID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Salon");
                });

            modelBuilder.Entity("SalonYonetimUygulamasi.Models.Randevu", b =>
                {
                    b.HasOne("SalonYonetimUygulamasi.Models.Calisan", "Calisan")
                        .WithMany()
                        .HasForeignKey("CalisanID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Calisan");
                });

            modelBuilder.Entity("SalonYonetimUygulamasi.Models.Salon", b =>
                {
                    b.Navigation("Calisanlar");
                });
#pragma warning restore 612, 618
        }
    }
}
