﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HaManhKien_11.Migrations
{
    [DbContext(typeof(LTQLDb))]
    [Migration("20240612040056_Update_database")]
    partial class Update_database
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("HaManhKien_11.Models.LopHoc", b =>
                {
                    b.Property<int>("MaLop")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("TenLop")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("MaLop");

                    b.ToTable("LopHoc");
                });

            modelBuilder.Entity("HaManhKien_11.Models.SinhVien", b =>
                {
                    b.Property<string>("MaSinhVien")
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("HoTen")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int?>("MaLop")
                        .HasColumnType("INTEGER");

                    b.HasKey("MaSinhVien");

                    b.HasIndex("MaLop");

                    b.ToTable("SinhVien");
                });

            modelBuilder.Entity("HaManhKien_11.Models.SinhVien", b =>
                {
                    b.HasOne("HaManhKien_11.Models.LopHoc", "LopHoc")
                        .WithMany("SinhVien")
                        .HasForeignKey("MaLop")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("LopHoc");
                });

            modelBuilder.Entity("HaManhKien_11.Models.LopHoc", b =>
                {
                    b.Navigation("SinhVien");
                });
#pragma warning restore 612, 618
        }
    }
}
