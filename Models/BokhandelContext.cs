using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BokhandelApp.Models;

public partial class BokhandelContext : DbContext
{
    public BokhandelContext()
    {
    }

    public BokhandelContext(DbContextOptions<BokhandelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Beställningar> Beställningars { get; set; }

    public virtual DbSet<Beställningsdetaljer> Beställningsdetaljers { get; set; }

    public virtual DbSet<Butiker> Butikers { get; set; }

    public virtual DbSet<Böcker> Böckers { get; set; }

    public virtual DbSet<Författare> Författares { get; set; }

    public virtual DbSet<Kategorier> Kategoriers { get; set; }

    public virtual DbSet<Kunder> Kunders { get; set; }

    public virtual DbSet<LagerSaldo> LagerSaldos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=SOFIA;Database=Bokhandel;User Id=sa;Password=Admin123!;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Beställningar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Beställn__3214EC271DB75678");

            entity.ToTable("Beställningar");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.KundId).HasColumnName("KundID");
            entity.Property(e => e.TotalPris).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Kund).WithMany(p => p.Beställningars)
                .HasForeignKey(d => d.KundId)
                .HasConstraintName("FK__Beställni__KundI__76969D2E");
        });

        modelBuilder.Entity<Beställningsdetaljer>(entity =>
        {
            entity.HasKey(e => new { e.BeställningId, e.Isbn }).HasName("PK__Beställn__7F5FB19F5C67FDAF");

            entity.ToTable("Beställningsdetaljer");

            entity.Property(e => e.BeställningId).HasColumnName("BeställningID");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN");
            entity.Property(e => e.Pris).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Beställning).WithMany(p => p.Beställningsdetaljers)
                .HasForeignKey(d => d.BeställningId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Beställni__Bestä__797309D9");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.Beställningsdetaljers)
                .HasForeignKey(d => d.Isbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Beställnin__ISBN__7A672E12");
        });

        modelBuilder.Entity<Butiker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Butiker__3214EC27A87D56BA");

            entity.ToTable("Butiker");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Adress).HasMaxLength(200);
            entity.Property(e => e.Butiksnamn).HasMaxLength(100);
        });

        modelBuilder.Entity<Böcker>(entity =>
        {
            entity.HasKey(e => e.Isbn).HasName("PK__Böcker__447D36EBE0F359DF");

            entity.ToTable("Böcker");

            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN");
            entity.Property(e => e.FörfattareId).HasColumnName("FörfattareID");
            entity.Property(e => e.Pris).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Språk).HasMaxLength(50);
            entity.Property(e => e.Titel).HasMaxLength(100);

            entity.HasOne(d => d.Författare).WithMany(p => p.Böckers)
                .HasForeignKey(d => d.FörfattareId)
                .HasConstraintName("FK__Böcker__Författa__398D8EEE");

            entity.HasMany(d => d.Kategoris).WithMany(p => p.Isbns)
                .UsingEntity<Dictionary<string, object>>(
                    "BokKategorier",
                    r => r.HasOne<Kategorier>().WithMany()
                        .HasForeignKey("KategoriId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__BokKatego__Kateg__01142BA1"),
                    l => l.HasOne<Böcker>().WithMany()
                        .HasForeignKey("Isbn")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__BokKategor__ISBN__00200768"),
                    j =>
                    {
                        j.HasKey("Isbn", "KategoriId").HasName("PK__BokKateg__75051A229C40ED27");
                        j.ToTable("BokKategorier");
                        j.IndexerProperty<string>("Isbn")
                            .HasMaxLength(13)
                            .IsUnicode(false)
                            .IsFixedLength()
                            .HasColumnName("ISBN");
                        j.IndexerProperty<int>("KategoriId").HasColumnName("KategoriID");
                    });
        });

        modelBuilder.Entity<Författare>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Författa__3214EC27CD4A5983");

            entity.ToTable("Författare");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Efternamn).HasMaxLength(50);
            entity.Property(e => e.Förnamn).HasMaxLength(50);
        });

        modelBuilder.Entity<Kategorier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Kategori__3214EC27BE860E68");

            entity.ToTable("Kategorier");

            entity.HasIndex(e => e.Namn, "UQ__Kategori__737584FD102F7496").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Namn).HasMaxLength(50);
        });

        modelBuilder.Entity<Kunder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Kunder__3214EC276B3D5B59");

            entity.ToTable("Kunder");

            entity.HasIndex(e => e.Epost, "UQ__Kunder__0CCE4D17176E6071").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Efternamn).HasMaxLength(50);
            entity.Property(e => e.Epost).HasMaxLength(100);
            entity.Property(e => e.Förnamn).HasMaxLength(50);
            entity.Property(e => e.Telefon).HasMaxLength(15);
        });

        modelBuilder.Entity<LagerSaldo>(entity =>
        {
            entity.HasKey(e => new { e.ButikId, e.Isbn }).HasName("PK__LagerSal__1191B894459D81BB");

            entity.ToTable("LagerSaldo");

            entity.Property(e => e.ButikId).HasColumnName("ButikID");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN");

            entity.HasOne(d => d.Butik).WithMany(p => p.LagerSaldos)
                .HasForeignKey(d => d.ButikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LagerSald__Butik__3E52440B");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.LagerSaldos)
                .HasForeignKey(d => d.Isbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LagerSaldo__ISBN__3F466844");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
