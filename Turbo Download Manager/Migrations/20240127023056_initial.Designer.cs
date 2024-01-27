﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Turbo_Download_Manager.Database;

#nullable disable

namespace Turbo_Download_Manager.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20240127023056_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("Turbo_Download_Manager.Models.FileDownloadEntry", b =>
                {
                    b.Property<string>("FileId")
                        .HasColumnType("TEXT");

                    b.Property<string>("DownloadUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .HasColumnType("TEXT");

                    b.Property<string>("SavePath")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDownloadDateTime")
                        .HasColumnType("TEXT");

                    b.HasKey("FileId");

                    b.ToTable("Items");
                });
#pragma warning restore 612, 618
        }
    }
}