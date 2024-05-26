using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
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
                string connectionString = "";
                try
                {
                    connectionString = File.ReadAllText("../../../../connection.txt");
                }
                catch
                {
                    return;
                }

                if (connectionString == null || connectionString == "")
                {
                    //Standard Connection String
                    optionsBuilder.UseSqlServer("Data Source=LAPTOP-CHRISTIA\\SQLEXPRESS;Initial Catalog=StockControl;Integrated Security=SSPI");
                }
                else
                {
                    //Connection String aus der Datei gelesen
                    optionsBuilder.UseSqlServer("Data Source=" + connectionString + ";Initial Catalog=StockControl;Integrated Security=SSPI");
                }
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

                entity.Property(e => e.Passwort).HasMaxLength(255);

                entity.Property(e => e.Rolle).HasMaxLength(255);

                entity.Property(e => e.Telefon).HasMaxLength(255);
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
                    .HasConstraintName("FK__Lager__BenutzerI__02925FBF");
            });

            modelBuilder.Entity<Lieferant>(entity =>
            {
                entity.HasKey(e => e.LieferantenId)
                    .HasName("PK__Lieferan__884AB8526E462E20");

                entity.ToTable("Lieferant");

                entity.Property(e => e.LieferantenId).HasColumnName("LieferantenID");

                entity.Property(e => e.Adresse).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Telefon).HasMaxLength(50);
            });

            modelBuilder.Entity<LieferantenWare>(entity =>
            {
                entity.HasKey(e => new { e.LieferantenId, e.WarenId })
                    .HasName("PK__Lieferan__EA93CEDC6DC614C6");

                entity.ToTable("Lieferanten_Ware");

                entity.Property(e => e.LieferantenId).HasColumnName("LieferantenID");

                entity.Property(e => e.WarenId).HasColumnName("WarenID");

                entity.Property(e => e.Lieferdatum).HasColumnType("date");

                entity.Property(e => e.Preis).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Lieferanten)
                    .WithMany(p => p.LieferantenWares)
                    .HasForeignKey(d => d.LieferantenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lieferant__Liefe__093F5D4E");

                entity.HasOne(d => d.Waren)
                    .WithMany(p => p.LieferantenWares)
                    .HasForeignKey(d => d.WarenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lieferant__Waren__0A338187");
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
