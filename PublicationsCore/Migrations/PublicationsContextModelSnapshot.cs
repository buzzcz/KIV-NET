﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using MySql.Data.EntityFrameworkCore.Storage.Internal;
using PublicationsCore.Persistence;
using System;

namespace PublicationsCore.Migrations
{
    [DbContext(typeof(PublicationsContext))]
    partial class PublicationsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("PublicationsCore.Persistence.Model.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("PublicationsCore.Persistence.Model.AuthorPublication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AuthorId");

                    b.Property<int>("PublicationId");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("PublicationId");

                    b.ToTable("AuthorPublications");
                });

            modelBuilder.Entity("PublicationsCore.Persistence.Model.Publication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(3)");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Edition")
                        .IsRequired();

                    b.Property<int?>("PublisherId");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("PublisherId");

                    b.ToTable("Publications");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Publication");
                });

            modelBuilder.Entity("PublicationsCore.Persistence.Model.Publisher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("PublicationsCore.Persistence.Model.Article", b =>
                {
                    b.HasBaseType("PublicationsCore.Persistence.Model.Publication");

                    b.Property<string>("Doi");

                    b.Property<string>("Issn");

                    b.Property<string>("MagazineTitle")
                        .IsRequired();

                    b.Property<string>("Pages")
                        .IsRequired();

                    b.Property<int>("Volume");

                    b.ToTable("Article");

                    b.HasDiscriminator().HasValue("Article");
                });

            modelBuilder.Entity("PublicationsCore.Persistence.Model.Book", b =>
                {
                    b.HasBaseType("PublicationsCore.Persistence.Model.Publication");

                    b.Property<string>("Isbn")
                        .IsRequired();

                    b.ToTable("Book");

                    b.HasDiscriminator().HasValue("Book");
                });

            modelBuilder.Entity("PublicationsCore.Persistence.Model.AuthorPublication", b =>
                {
                    b.HasOne("PublicationsCore.Persistence.Model.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PublicationsCore.Persistence.Model.Publication")
                        .WithMany("AuthorPublicationList")
                        .HasForeignKey("PublicationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PublicationsCore.Persistence.Model.Publication", b =>
                {
                    b.HasOne("PublicationsCore.Persistence.Model.Publisher", "Publisher")
                        .WithMany()
                        .HasForeignKey("PublisherId");
                });
#pragma warning restore 612, 618
        }
    }
}
