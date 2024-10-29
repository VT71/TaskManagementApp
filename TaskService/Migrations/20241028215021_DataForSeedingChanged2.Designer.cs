﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskService.Data;

#nullable disable

namespace TaskService.Migrations
{
    [DbContext(typeof(ToDoTaskContext))]
    [Migration("20241028215021_DataForSeedingChanged2")]
    partial class DataForSeedingChanged2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("TaskService.Models.ToDoTask", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Completed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ToDoTasks");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Completed = false,
                            DueDate = new DateTime(2025, 10, 27, 15, 23, 59, 689, DateTimeKind.Local),
                            Title = "Task 1"
                        },
                        new
                        {
                            Id = 2L,
                            Completed = true,
                            DueDate = new DateTime(2025, 10, 27, 15, 23, 59, 689, DateTimeKind.Local),
                            Title = "Task 2"
                        },
                        new
                        {
                            Id = 3L,
                            Completed = false,
                            DueDate = new DateTime(2025, 10, 27, 15, 23, 59, 689, DateTimeKind.Local),
                            Title = "Task 3"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}