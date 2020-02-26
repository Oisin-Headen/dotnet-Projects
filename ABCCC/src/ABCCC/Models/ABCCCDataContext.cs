using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ABCCC.Models
{
    public partial class ABCCCDataContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Cineplex> Cineplex { get; set; }
        public virtual DbSet<CineplexMovie> CineplexMovie { get; set; }
        public virtual DbSet<Enquiry> Enquiry { get; set; }
        public virtual DbSet<AbstractMovie> AllMovies { get; set; }
        public virtual DbSet<Movie> Movie { get; set; }
        public virtual DbSet<MovieComingSoon> MovieComingSoon { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<CCInformation> CreditCards { get; set; }
        public virtual DbSet<MovieBooking> Bookings { get; set; }

        public ABCCCDataContext(DbContextOptions<ABCCCDataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cineplex>(entity =>
            {
                entity.Property(e => e.CineplexId).HasColumnName("CineplexID");

                entity.Property(e => e.Location).IsRequired();

                entity.Property(e => e.LongDescription).IsRequired();

                entity.Property(e => e.ShortDescription).IsRequired();
            });

            modelBuilder.Entity<CineplexMovie>(entity =>
            {
                // Each Cineplex has only one theater, so only one movie can be playing at a time.
                // Therefore, the key does not require a MovieId
                entity.HasKey(e => new { e.CineplexId, e.Day, e.Hour, e.Period })
                    .HasName("PK__Cineplex__CB419E6DBC409681");

                entity.Property(cm => cm.AvailableSeats).HasColumnName("Availible Seats").IsRequired();

                entity.Property(e => e.CineplexId).HasColumnName("CineplexID");

                entity.Property(e => e.MovieId).HasColumnName("MovieID").IsRequired();

                //There are no navagation properties in the Movie and Cineplex classes, they are not needed.
                entity.HasOne(d => d.Cineplex)
                    .WithMany(c => c.CineplexMovie)
                    .HasForeignKey(d => d.CineplexId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__CineplexM__Cinep__35BCFE0A");

                entity.HasOne(d => d.Movie)
                    .WithMany(m => m.CineplexMovie)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__CineplexM__Movie__36B12243");
            });

            modelBuilder.Entity<Enquiry>(entity =>
            {
                entity.Property(e => e.EnquiryId).HasColumnName("EnquiryID");

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.Message).IsRequired();
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.MovieId).HasColumnName("MovieID");

                entity.Property(e => e.LongDescription).IsRequired();

                entity.Property(e => e.ShortDescription).IsRequired();

                entity.Property(e => e.Title).IsRequired();
            });

            modelBuilder.Entity<MovieComingSoon>(entity =>
            {
                entity.Property(e => e.MovieId).HasColumnName("MovieID");

                entity.Property(e => e.LongDescription).IsRequired();

                entity.Property(e => e.ShortDescription).IsRequired();

                entity.Property(e => e.Title).IsRequired();

                entity.Property(e => e.ReleaseDate).IsRequired();
            });

            modelBuilder.Entity<AbstractMovie>(entity =>
            {
                entity.HasKey(m => new { m.MovieId });
                entity.Property(e => e.MovieId).HasColumnName("MovieID");

                entity.Property(e => e.LongDescription).IsRequired();

                entity.Property(e => e.ShortDescription).IsRequired();

                entity.Property(e => e.Title).IsRequired();
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(o => new { o.TransactionId });
            });

            modelBuilder.Entity<CCInformation>(entity =>
            {
                entity.HasKey(cc => new { cc.CreditCardId });

                entity.HasOne(cc => cc.Transaction)
                    .WithOne(o => o.CreditCard)
                    .HasForeignKey<CCInformation>(cc => cc.OrderId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Order_To_CreditCard");
            });

            modelBuilder.Entity<MovieBooking>(entity =>
            {
                entity.HasKey(mb => new { mb.BookingId });

                entity.HasOne(mb => mb.Transaction)
                    .WithMany(o => o.Bookings)
                    .HasForeignKey(mb => mb.OrderId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Order_To_Bookings");

                entity.HasOne(mb => mb.Cineplex)
                    .WithMany()
                    .HasForeignKey(mb => mb.CineplexId)
                    .HasConstraintName("FK_Bookings_To_Cineplex");
            });
        }
    }
}