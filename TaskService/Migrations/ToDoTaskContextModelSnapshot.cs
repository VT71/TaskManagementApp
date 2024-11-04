﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskService.Data;

#nullable disable

namespace TaskService.Migrations
{
    [DbContext(typeof(ToDoTaskContext))]
    partial class ToDoTaskContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean ut velit vel orci dapibus tristique.",
                            DueDate = new DateTime(2025, 1, 1, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task A"
                        },
                        new
                        {
                            Id = 2L,
                            Completed = true,
                            DueDate = new DateTime(2025, 1, 2, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task B"
                        },
                        new
                        {
                            Id = 3L,
                            Completed = false,
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque non orci sit amet turpis cursus.",
                            DueDate = new DateTime(2025, 1, 3, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task C"
                        },
                        new
                        {
                            Id = 4L,
                            Completed = true,
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut a massa nec elit vestibulum aliquam.",
                            DueDate = new DateTime(2025, 1, 4, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task D"
                        },
                        new
                        {
                            Id = 5L,
                            Completed = false,
                            DueDate = new DateTime(2025, 1, 5, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task E"
                        },
                        new
                        {
                            Id = 6L,
                            Completed = true,
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi volutpat tortor et magna bibendum.",
                            DueDate = new DateTime(2025, 1, 6, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task F"
                        },
                        new
                        {
                            Id = 7L,
                            Completed = false,
                            DueDate = new DateTime(2025, 1, 7, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task G"
                        },
                        new
                        {
                            Id = 8L,
                            Completed = true,
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse a risus ac sapien ultricies.",
                            DueDate = new DateTime(2025, 1, 8, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task H"
                        },
                        new
                        {
                            Id = 9L,
                            Completed = false,
                            DueDate = new DateTime(2025, 1, 9, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task I"
                        },
                        new
                        {
                            Id = 10L,
                            Completed = true,
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent vehicula nisl at elit feugiat.",
                            DueDate = new DateTime(2025, 1, 10, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task J"
                        },
                        new
                        {
                            Id = 11L,
                            Completed = false,
                            DueDate = new DateTime(2025, 1, 11, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task K"
                        },
                        new
                        {
                            Id = 12L,
                            Completed = true,
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur auctor orci sit amet mi.",
                            DueDate = new DateTime(2025, 1, 12, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task L"
                        },
                        new
                        {
                            Id = 13L,
                            Completed = false,
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec tempor ligula eget cursus.",
                            DueDate = new DateTime(2025, 1, 13, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task M"
                        },
                        new
                        {
                            Id = 14L,
                            Completed = true,
                            DueDate = new DateTime(2025, 1, 14, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task N"
                        },
                        new
                        {
                            Id = 15L,
                            Completed = false,
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus scelerisque orci vitae metus.",
                            DueDate = new DateTime(2025, 1, 15, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task O"
                        },
                        new
                        {
                            Id = 16L,
                            Completed = true,
                            DueDate = new DateTime(2025, 1, 16, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task P"
                        },
                        new
                        {
                            Id = 17L,
                            Completed = false,
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer at justo vel erat pharetra.",
                            DueDate = new DateTime(2025, 1, 17, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task Q"
                        },
                        new
                        {
                            Id = 18L,
                            Completed = true,
                            DueDate = new DateTime(2025, 1, 18, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task R"
                        },
                        new
                        {
                            Id = 19L,
                            Completed = false,
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam et lacus pulvinar, porta eros.",
                            DueDate = new DateTime(2025, 1, 19, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task S"
                        },
                        new
                        {
                            Id = 20L,
                            Completed = true,
                            DueDate = new DateTime(2025, 1, 20, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task T"
                        },
                        new
                        {
                            Id = 21L,
                            Completed = false,
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum sit amet lacus lacinia.",
                            DueDate = new DateTime(2025, 1, 21, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task U"
                        },
                        new
                        {
                            Id = 22L,
                            Completed = true,
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam non justo at orci commodo.",
                            DueDate = new DateTime(2025, 1, 22, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task V"
                        },
                        new
                        {
                            Id = 23L,
                            Completed = false,
                            DueDate = new DateTime(2025, 1, 23, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task W"
                        },
                        new
                        {
                            Id = 24L,
                            Completed = true,
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus consectetur orci id odio.",
                            DueDate = new DateTime(2025, 1, 24, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task X"
                        },
                        new
                        {
                            Id = 25L,
                            Completed = false,
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin sed felis vestibulum, lacinia.",
                            DueDate = new DateTime(2025, 1, 25, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task Y"
                        },
                        new
                        {
                            Id = 26L,
                            Completed = true,
                            DueDate = new DateTime(2025, 1, 26, 2, 0, 0, 0, DateTimeKind.Local),
                            Title = "Task Z"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
