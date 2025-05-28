using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Investly.DAL.Entities;

public partial class AppDbContext : DbContext
{
    

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Founder> Founders { get; set; }

    public virtual DbSet<Government> Governments { get; set; }

    public virtual DbSet<Investor> Investors { get; set; }

    public virtual DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cities__3214EC075E53FA05");

            entity.Property(e => e.NameAr)
                .HasMaxLength(200)
                .HasColumnName("Name_Ar");
            entity.Property(e => e.NameEn)
                .HasMaxLength(200)
                .HasColumnName("Name_En");

            entity.HasOne(d => d.Gov).WithMany(p => p.Cities)
                .HasForeignKey(d => d.GovId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cities__GovId__398D8EEE");
        });

        modelBuilder.Entity<Founder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Founders__3214EC07E05ED713");

            entity.HasOne(d => d.User).WithMany(p => p.Founders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Founders__UserId__412EB0B6");
        });

        modelBuilder.Entity<Government>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Governme__3214EC0735A35290");

            entity.Property(e => e.NameAr)
                .HasMaxLength(200)
                .HasColumnName("Name_Ar");
            entity.Property(e => e.NameEn)
                .HasMaxLength(200)
                .HasColumnName("Name_En");
        });

        modelBuilder.Entity<Investor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Investor__3214EC070C4CF063");

            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.User).WithMany(p => p.Investors)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Investors__User___440B1D61");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0776B9F427");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053421BB6CBF").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.BackIdPicPath).IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.FrontIdPicPath).IsUnicode(false);
            entity.Property(e => e.HashedPassword)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.NationalId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.City).WithMany(p => p.Users)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__Users__CityId__3E52440B");

            entity.HasOne(d => d.Government).WithMany(p => p.Users)
                .HasForeignKey(d => d.GovernmentId)
                .HasConstraintName("FK__Users__Governmen__3D5E1FD2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
