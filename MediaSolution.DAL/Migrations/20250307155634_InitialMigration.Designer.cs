﻿// <auto-generated />
using System;
using MediaSolution.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MediaSolution.DAL.Migrations
{
    [DbContext(typeof(MediaSolutionDbContext))]
    [Migration("20250307155634_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("MediaSolution.DAL.Entities.MediaEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Authors")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("TEXT");

                    b.Property<string>("Genres")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("SizeInBytes")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MediaEntities");
                });

            modelBuilder.Entity("MediaSolution.DAL.Entities.PlaylistEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("CoverImage")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Favorite")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PlaylistEntities");
                });

            modelBuilder.Entity("MediaSolution.DAL.Entities.PlaylistMediaEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MediaId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("PlaylistId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MediaId");

                    b.HasIndex("PlaylistId");

                    b.ToTable("PlaylistMediaEntities");
                });

            modelBuilder.Entity("MediaSolution.DAL.Entities.PlaylistMediaEntity", b =>
                {
                    b.HasOne("MediaSolution.DAL.Entities.MediaEntity", "Media")
                        .WithMany("Playlists")
                        .HasForeignKey("MediaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MediaSolution.DAL.Entities.PlaylistEntity", "Playlist")
                        .WithMany("Media")
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Media");

                    b.Navigation("Playlist");
                });

            modelBuilder.Entity("MediaSolution.DAL.Entities.MediaEntity", b =>
                {
                    b.Navigation("Playlists");
                });

            modelBuilder.Entity("MediaSolution.DAL.Entities.PlaylistEntity", b =>
                {
                    b.Navigation("Media");
                });
#pragma warning restore 612, 618
        }
    }
}
