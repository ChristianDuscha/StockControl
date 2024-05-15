using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace StockControl.Models
{
    public partial class StockControlContext : DbContext
    {
        public StockControlContext()
        {
        }

        public StockControlContext(DbContextOptions<StockControlContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Benutzer> Benutzers { get; set; } = null!;
        public virtual DbSet<Lager> Lagers { get; set; } = null!;
        public virtual DbSet<Lieferant> Lieferants { get; set; } = null!;
        public virtual DbSet<LieferantenWare> LieferantenWares { get; set; } = null!;
        public virtual DbSet<Waren> Warens { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=QRIZZPC\\SQLEXPRESS;Initial Catalog=StockControl;Integrated Security=SSPI");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Benutzer>(entity =>
            {
                entity.ToTable("Benutzer");

                entity.Property(e => e.BenutzerId).HasColumnName("BenutzerID");

                entity.Property(e => e.Adresse).HasMaxLength(255);

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("EMail");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Rolle).HasMaxLength(255);

                entity.Property(e => e.Telefon).HasMaxLength(50);
            });

            modelBuilder.Entity<Lager>(entity =>
            {
                entity.ToTable("Lager");

                entity.Property(e => e.LagerId).HasColumnName("LagerID");

                entity.Property(e => e.BenutzerId).HasColumnName("BenutzerID");

                entity.Property(e => e.Bestand).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Lagername).HasMaxLength(255);

                entity.Property(e => e.Standort).HasMaxLength(255);

                entity.HasOne(d => d.Benutzer)
                    .WithMany(p => p.Lagers)
                    .HasForeignKey(d => d.BenutzerId)
                    .HasConstraintName("FK__Lager__BenutzerI__41EDCAC5");
            });

            modelBuilder.Entity<Lieferant>(entity =>
            {
                entity.HasKey(e => e.LieferantenId)
                    .HasName("PK__Lieferan__884AB852BE87CF4E");

                entity.ToTable("Lieferant");

                entity.Property(e => e.LieferantenId).HasColumnName("LieferantenID");

                entity.Property(e => e.Adresse).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Telefon).HasMaxLength(50);
            });

            modelBuilder.Entity<LieferantenWare>(entity =>
            {
                entity.HasKey(e => new { e.LieferantenId, e.WarenId })
                    .HasName("PK__Lieferan__EA93CEDC2BC9EF51");

                entity.ToTable("Lieferanten_Ware");

                entity.Property(e => e.LieferantenId).HasColumnName("LieferantenID");

                entity.Property(e => e.WarenId).HasColumnName("WarenID");

                entity.Property(e => e.Lieferdatum).HasColumnType("date");

                entity.Property(e => e.Preis).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Lieferanten)
                    .WithMany(p => p.LieferantenWares)
                    .HasForeignKey(d => d.LieferantenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lieferant__Liefe__489AC854");

                entity.HasOne(d => d.Waren)
                    .WithMany(p => p.LieferantenWares)
                    .HasForeignKey(d => d.WarenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lieferant__Waren__498EEC8D");
            });

            modelBuilder.Entity<Waren>(entity =>
            {
                entity.ToTable("Waren");

                entity.Property(e => e.WarenId).HasColumnName("WarenID");

                entity.Property(e => e.Warennamen).HasMaxLength(255);

                entity.Property(e => e.Warentyp).HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
