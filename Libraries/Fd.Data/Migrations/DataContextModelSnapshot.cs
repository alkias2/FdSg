﻿// <auto-generated />
using System;
using Fd.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Fd.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Fd.Data.Domain.Bait", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Preparation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Bait");
                });

            modelBuilder.Entity("Fd.Data.Domain.Diary", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Bottomorph")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("Success")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Diary");
                });

            modelBuilder.Entity("Fd.Data.Domain.Location", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("District")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Lat")
                        .HasColumnType("float");

                    b.Property<double?>("Lng")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("Fd.Data.Domain.SgData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<long>("LocationId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RowData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("SgData");
                });

            modelBuilder.Entity("Fd.Data.Domain.Solunar", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime?>("CivilDawn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CivilDusk")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<long>("LocationId")
                        .HasColumnType("bigint");

                    b.Property<string>("MoonClosestName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("MoonClosestTime")
                        .HasColumnType("datetime2");

                    b.Property<double?>("MoonClosestValue")
                        .HasColumnType("float");

                    b.Property<string>("MoonCurrentName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("MoonCurrentTime")
                        .HasColumnType("datetime2");

                    b.Property<double?>("MoonCurrenttValue")
                        .HasColumnType("float");

                    b.Property<double?>("MoonFraction")
                        .HasColumnType("float");

                    b.Property<DateTime?>("MoonRise")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("MoonSet")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("SunRise")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("SunSet")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Solunar");
                });

            modelBuilder.Entity("Fd.Data.Domain.Tide", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double?>("Height")
                        .HasColumnType("float");

                    b.Property<long>("LocationId")
                        .HasColumnType("bigint");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Tide");
                });

            modelBuilder.Entity("Fd.Data.Domain.TripResult", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("BaitId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CatchTime")
                        .HasColumnType("datetime2");

                    b.Property<long?>("DiaryId")
                        .HasColumnType("bigint");

                    b.Property<long>("DiraryId")
                        .HasColumnType("bigint");

                    b.Property<double?>("FightTime")
                        .HasColumnType("float");

                    b.Property<long>("FishId")
                        .HasColumnType("bigint");

                    b.Property<bool>("Release")
                        .HasColumnType("bit");

                    b.Property<int>("Techinique")
                        .HasColumnType("int");

                    b.Property<double?>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("DiaryId");

                    b.ToTable("TripResult");
                });

            modelBuilder.Entity("Fd.Data.Domain.Whether", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<double?>("AirTemperature")
                        .HasColumnType("float");

                    b.Property<double?>("CloudCover")
                        .HasColumnType("float");

                    b.Property<double?>("CurrentDirection")
                        .HasColumnType("float");

                    b.Property<double?>("CurrentSpeed")
                        .HasColumnType("float");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double?>("Gust")
                        .HasColumnType("float");

                    b.Property<double?>("Humidity")
                        .HasColumnType("float");

                    b.Property<long>("LocationId")
                        .HasColumnType("bigint");

                    b.Property<double?>("Pressure")
                        .HasColumnType("float");

                    b.Property<double?>("SeaLevel")
                        .HasColumnType("float");

                    b.Property<double?>("SwellDirection")
                        .HasColumnType("float");

                    b.Property<double?>("SwellHeight")
                        .HasColumnType("float");

                    b.Property<double?>("SwellPeriod")
                        .HasColumnType("float");

                    b.Property<double?>("waterTemperature")
                        .HasColumnType("float");

                    b.Property<double?>("waveDirection")
                        .HasColumnType("float");

                    b.Property<double?>("waveHeight")
                        .HasColumnType("float");

                    b.Property<double?>("wavePeriod")
                        .HasColumnType("float");

                    b.Property<double?>("windDirection")
                        .HasColumnType("float");

                    b.Property<double?>("windSpeed")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Whether");
                });

            modelBuilder.Entity("Fd.Data.Domain.SgData", b =>
                {
                    b.HasOne("Fd.Data.Domain.Location", null)
                        .WithMany("SgDatas")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fd.Data.Domain.Solunar", b =>
                {
                    b.HasOne("Fd.Data.Domain.Location", null)
                        .WithMany("Solunars")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fd.Data.Domain.Tide", b =>
                {
                    b.HasOne("Fd.Data.Domain.Location", null)
                        .WithMany("Tides")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fd.Data.Domain.TripResult", b =>
                {
                    b.HasOne("Fd.Data.Domain.Diary", null)
                        .WithMany("TripResults")
                        .HasForeignKey("DiaryId");
                });

            modelBuilder.Entity("Fd.Data.Domain.Whether", b =>
                {
                    b.HasOne("Fd.Data.Domain.Location", null)
                        .WithMany("Whethers")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fd.Data.Domain.Diary", b =>
                {
                    b.Navigation("TripResults");
                });

            modelBuilder.Entity("Fd.Data.Domain.Location", b =>
                {
                    b.Navigation("SgDatas");

                    b.Navigation("Solunars");

                    b.Navigation("Tides");

                    b.Navigation("Whethers");
                });
#pragma warning restore 612, 618
        }
    }
}
