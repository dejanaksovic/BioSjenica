﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using bioSjenica.Data;

#nullable disable

namespace bioSjenica.Migrations
{
    [DbContext(typeof(SqlContext))]
    [Migration("20240531125221_UserTableAdded")]
    partial class UserTableAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AnimalFeedingGround", b =>
                {
                    b.Property<int>("AnimalsId")
                        .HasColumnType("int");

                    b.Property<int>("FeedingGroundsId")
                        .HasColumnType("int");

                    b.HasKey("AnimalsId", "FeedingGroundsId");

                    b.HasIndex("FeedingGroundsId");

                    b.ToTable("AnimalFeedingGround");
                });

            modelBuilder.Entity("AnimalRegion", b =>
                {
                    b.Property<int>("AnimalsId")
                        .HasColumnType("int");

                    b.Property<int>("RegionsId")
                        .HasColumnType("int");

                    b.HasKey("AnimalsId", "RegionsId");

                    b.HasIndex("RegionsId");

                    b.ToTable("AnimalRegion");
                });

            modelBuilder.Entity("PlantRegion", b =>
                {
                    b.Property<int>("PlantsId")
                        .HasColumnType("int");

                    b.Property<int>("RegionsId")
                        .HasColumnType("int");

                    b.HasKey("PlantsId", "RegionsId");

                    b.HasIndex("RegionsId");

                    b.ToTable("PlantRegion");
                });

            modelBuilder.Entity("bioSjenica.Models.Animal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CommonName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LatinicName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RingNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RingNumber", "LatinicName", "CommonName")
                        .IsUnique();

                    b.ToTable("Animals");
                });

            modelBuilder.Entity("bioSjenica.Models.FeedingGround", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EndWork")
                        .HasColumnType("int");

                    b.Property<int>("GroundNumber")
                        .HasColumnType("int");

                    b.Property<int>("RegionId")
                        .HasColumnType("int");

                    b.Property<int>("StartWork")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroundNumber")
                        .IsUnique();

                    b.HasIndex("RegionId");

                    b.ToTable("FeedingGorunds");
                });

            modelBuilder.Entity("bioSjenica.Models.Plant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CommonName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LatinicName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SpecialDecision")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("SpecialTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("LatinicName", "CommonName", "ImageUrl")
                        .IsUnique();

                    b.ToTable("Plants");
                });

            modelBuilder.Entity("bioSjenica.Models.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("Area")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProtectionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Villages")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("bioSjenica.Models.User", b =>
                {
                    b.Property<string>("SSN")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FistName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("PayGrade")
                        .HasColumnType("real");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SSN");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AnimalFeedingGround", b =>
                {
                    b.HasOne("bioSjenica.Models.Animal", null)
                        .WithMany()
                        .HasForeignKey("AnimalsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("bioSjenica.Models.FeedingGround", null)
                        .WithMany()
                        .HasForeignKey("FeedingGroundsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AnimalRegion", b =>
                {
                    b.HasOne("bioSjenica.Models.Animal", null)
                        .WithMany()
                        .HasForeignKey("AnimalsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("bioSjenica.Models.Region", null)
                        .WithMany()
                        .HasForeignKey("RegionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PlantRegion", b =>
                {
                    b.HasOne("bioSjenica.Models.Plant", null)
                        .WithMany()
                        .HasForeignKey("PlantsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("bioSjenica.Models.Region", null)
                        .WithMany()
                        .HasForeignKey("RegionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("bioSjenica.Models.FeedingGround", b =>
                {
                    b.HasOne("bioSjenica.Models.Region", "Region")
                        .WithMany("FeedingGrounds")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Region");
                });

            modelBuilder.Entity("bioSjenica.Models.Region", b =>
                {
                    b.Navigation("FeedingGrounds");
                });
#pragma warning restore 612, 618
        }
    }
}
