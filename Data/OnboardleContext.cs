using Microsoft.EntityFrameworkCore;
using Onboardle.Models;

namespace Onboardle.Data
{
    public class OnboardleContext: DbContext
    {
        public OnboardleContext(DbContextOptions<OnboardleContext> options) : base(options) {}

        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }
        public virtual DbSet<Track> Tracks { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<DriverTeamSeason> DriverTeamSeasons { get; set; }
        public virtual DbSet<TrackSeason> TrackSeasons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Photo>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Photo)
                    .WithOne(p => p.Game)
                    .HasForeignKey<Game>(e => e.PhotoId)
                    .IsRequired();
            });

            modelBuilder.Entity<Season>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Track>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Driver>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<DriverTeamSeason>(entity =>
            {
                entity.HasKey(e => new { e.DriverId, e.TeamId, e.SeasonId });
                entity.HasOne(e => e.Season)
                    .WithMany()
                    .HasForeignKey(e => e.SeasonId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TrackSeason>(entity =>
            {
                entity.HasKey(e => new { e.TrackId, e.SeasonId });
                entity.HasOne(e => e.Season)
                .WithMany()
                .HasForeignKey(e => e.SeasonId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
