﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ABCCC.Models;

namespace ABCCC.Migrations
{
    [DbContext(typeof(ABCCCDataContext))]
    [Migration("20170816003534_CreditCardUpdate")]
    partial class CreditCardUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ABCCC.Models.AbstractMovie", b =>
                {
                    b.Property<int>("MovieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("MovieID");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("ImageUrl");

                    b.Property<string>("LongDescription")
                        .IsRequired();

                    b.Property<string>("ShortDescription")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("MovieId");

                    b.ToTable("AllMovies");

                    b.HasDiscriminator<string>("Discriminator").HasValue("AbstractMovie");
                });

            modelBuilder.Entity("ABCCC.Models.CCInformation", b =>
                {
                    b.Property<int>("CreditCardId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CCV");

                    b.Property<int>("ExpiryMonth");

                    b.Property<int>("ExpiryYear");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Number");

                    b.Property<int>("OrderId");

                    b.HasKey("CreditCardId");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("CreditCards");
                });

            modelBuilder.Entity("ABCCC.Models.Cineplex", b =>
                {
                    b.Property<int>("CineplexId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CineplexID");

                    b.Property<string>("ImageUrl");

                    b.Property<string>("Location")
                        .IsRequired();

                    b.Property<string>("LongDescription")
                        .IsRequired();

                    b.Property<string>("ShortDescription")
                        .IsRequired();

                    b.HasKey("CineplexId");

                    b.ToTable("Cineplex");
                });

            modelBuilder.Entity("ABCCC.Models.CineplexMovie", b =>
                {
                    b.Property<int>("CineplexId")
                        .HasColumnName("CineplexID");

                    b.Property<int>("MovieId")
                        .HasColumnName("MovieID");

                    b.Property<int>("Day");

                    b.Property<int>("Hour");

                    b.Property<int>("Period");

                    b.HasKey("CineplexId", "MovieId", "Day", "Hour", "Period")
                        .HasName("PK__Cineplex__CB419E6DBC409681");

                    b.HasIndex("MovieId");

                    b.ToTable("CineplexMovie");
                });

            modelBuilder.Entity("ABCCC.Models.Enquiry", b =>
                {
                    b.Property<int>("EnquiryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("EnquiryID");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Message")
                        .IsRequired();

                    b.HasKey("EnquiryId");

                    b.ToTable("Enquiry");
                });

            modelBuilder.Entity("ABCCC.Models.MovieBooking", b =>
                {
                    b.Property<int>("BookingId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AdultSeats");

                    b.Property<int>("CineplexId");

                    b.Property<int>("ConcessionSeats");

                    b.Property<int>("Hour");

                    b.Property<int>("MovieId");

                    b.Property<int>("OrderId");

                    b.Property<int>("Period");

                    b.Property<int>("Day");

                    b.HasKey("BookingId");

                    b.HasIndex("OrderId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("ABCCC.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd();

                    b.HasKey("OrderId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ABCCC.Models.Movie", b =>
                {
                    b.HasBaseType("ABCCC.Models.AbstractMovie");


                    b.ToTable("Movie");

                    b.HasDiscriminator().HasValue("Movie");
                });

            modelBuilder.Entity("ABCCC.Models.MovieComingSoon", b =>
                {
                    b.HasBaseType("ABCCC.Models.AbstractMovie");

                    b.Property<string>("ReleaseDate")
                        .IsRequired();

                    b.ToTable("MovieComingSoon");

                    b.HasDiscriminator().HasValue("MovieComingSoon");
                });

            modelBuilder.Entity("ABCCC.Models.CCInformation", b =>
                {
                    b.HasOne("ABCCC.Models.Order", "Order")
                        .WithOne("CreditCard")
                        .HasForeignKey("ABCCC.Models.CCInformation", "OrderId")
                        .HasConstraintName("FK_Order_To_CreditCard");
                });

            modelBuilder.Entity("ABCCC.Models.CineplexMovie", b =>
                {
                    b.HasOne("ABCCC.Models.Cineplex", "Cineplex")
                        .WithMany("CineplexMovie")
                        .HasForeignKey("CineplexId")
                        .HasConstraintName("FK__CineplexM__Cinep__35BCFE0A");

                    b.HasOne("ABCCC.Models.Movie", "Movie")
                        .WithMany("CineplexMovie")
                        .HasForeignKey("MovieId");
                });

            modelBuilder.Entity("ABCCC.Models.MovieBooking", b =>
                {
                    b.HasOne("ABCCC.Models.Order", "Order")
                        .WithMany("Bookings")
                        .HasForeignKey("OrderId")
                        .HasConstraintName("FK_Order_To_Bookings");
                });
        }
    }
}
