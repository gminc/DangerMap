using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DangerMap.Models.db
{
    public partial class uploadtestv1Context : DbContext
    {
        public uploadtestv1Context()
        {
        }

        public uploadtestv1Context(DbContextOptions<uploadtestv1Context> options)
            : base(options)
        {
        }

        public virtual DbSet<AccountProfile> AccountProfiles { get; set; }
        public virtual DbSet<CriminalIncident> CriminalIncidents { get; set; }
        public virtual DbSet<EventDiscussion> EventDiscussions { get; set; }
        public virtual DbSet<EventWitness> EventWitnesses { get; set; }
        public virtual DbSet<InstantEvent> InstantEvents { get; set; }
        public virtual DbSet<NewsTicker> NewsTickers { get; set; }
        public virtual DbSet<TrafficAccident> TrafficAccidents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountProfile>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("PK_會員資料表");

                entity.ToTable("AccountProfile", "dbo");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(10)
                    .HasColumnName("AccountID");

                entity.Property(e => e.AccountEmail)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.PropicLink)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.RefreshToken)
                    .HasMaxLength(50)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<CriminalIncident>(entity =>
            {
                entity.HasKey(e => e.IncidentId)
                    .HasName("PK_2021Q3犯罪資料_1");

                entity.ToTable("CriminalIncident", "dbo");

                entity.Property(e => e.IncidentId)
                    .ValueGeneratedNever()
                    .HasColumnName("IncidentID");

                entity.Property(e => e.Location).HasMaxLength(50);

                entity.Property(e => e.Time).HasColumnType("smalldatetime");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<EventDiscussion>(entity =>
            {
                entity.HasKey(e => new { e.EventId, e.AccountId });

                entity.ToTable("EventDiscussion", "dbo");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(10)
                    .HasColumnName("AccountID");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CommentTime).HasColumnType("smalldatetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.EventDiscussions)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventDiscussion_AccountProfile");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventDiscussions)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventDiscussion_InstantEvent");
            });

            modelBuilder.Entity<EventWitness>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.EventId });

                entity.ToTable("EventWitness", "dbo");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(10)
                    .HasColumnName("AccountID");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.EventWitnesses)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventWitness_AccountProfile");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventWitnesses)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventWitness_InstantEvent");
            });

            modelBuilder.Entity<InstantEvent>(entity =>
            {
                entity.HasKey(e => e.EventId)
                    .HasName("PK_InstantEventNewID");

                entity.ToTable("InstantEvent", "dbo");

                entity.Property(e => e.EventId)
                    .ValueGeneratedNever()
                    .HasColumnName("EventID");

                entity.Property(e => e.AccountId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("AccountID");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.LocationDetails).HasMaxLength(50);

                entity.Property(e => e.ShotLink).HasMaxLength(200);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.UploadTime).HasColumnType("smalldatetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.InstantEvents)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InstantEvent_AccountProfile");
            });

            modelBuilder.Entity<NewsTicker>(entity =>
            {
                entity.HasKey(e => e.UploadTime);

                entity.ToTable("NewsTicker", "dbo");

                entity.Property(e => e.UploadTime).HasColumnType("datetime");

                entity.Property(e => e.InfoLink)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<TrafficAccident>(entity =>
            {
                entity.HasKey(e => e.AccidentId)
                    .HasName("PK_2021交通事故資料");

                entity.ToTable("TrafficAccident", "dbo");

                entity.Property(e => e.AccidentId)
                    .ValueGeneratedNever()
                    .HasColumnName("AccidentID");

                entity.Property(e => e.Time).HasColumnType("smalldatetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
