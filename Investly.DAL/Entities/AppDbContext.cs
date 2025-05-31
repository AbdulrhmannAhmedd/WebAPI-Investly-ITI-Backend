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

    public virtual DbSet<Business> Businesses { get; set; }

    public virtual DbSet<BusinessStandardAnswer> BusinessStandardAnswers { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryStandard> CategoryStandards { get; set; }

    public virtual DbSet<CategoryStandardsKeyWord> CategoryStandardsKeyWords { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<ContactU> ContactUs { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Founder> Founders { get; set; }

    public virtual DbSet<Government> Governments { get; set; }

    public virtual DbSet<Investor> Investors { get; set; }

    public virtual DbSet<InvestorContactRequest> InvestorContactRequests { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Standard> Standards { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Business__3214EC075E0D0000");

            entity.ToTable("Business");

            entity.Property(e => e.Airate)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("AIRate");
            entity.Property(e => e.Capital).HasColumnType("decimal(10, 5)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Businesses)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Business__Catego__7F2BE32F");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BusinessCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Business__Create__01142BA1");

            entity.HasOne(d => d.Founder).WithMany(p => p.Businesses)
                .HasForeignKey(d => d.FounderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Business__Founde__00200768");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.BusinessUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__Business__Update__02084FDA");
        });

        modelBuilder.Entity<BusinessStandardAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Business__3214EC074A4C9C5D");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Business).WithMany(p => p.BusinessStandardAnswers)
                .HasForeignKey(d => d.BusinessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BusinessS__Busin__10566F31");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BusinessStandardAnswerCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__BusinessS__Creat__114A936A");

            entity.HasOne(d => d.Standard).WithMany(p => p.BusinessStandardAnswers)
                .HasForeignKey(d => d.StandardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BusinessS__Stand__0F624AF8");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.BusinessStandardAnswerUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__BusinessS__Updat__123EB7A3");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC074C9193E2");

            entity.ToTable("Category");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CategoryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Category__Create__6FE99F9F");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CategoryUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__Category__Update__70DDC3D8");
        });

        modelBuilder.Entity<CategoryStandard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07756124F4");

            entity.ToTable("CategoryStandard");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.CategoryStandards)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CategoryS__Categ__797309D9");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CategoryStandardCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__CategoryS__Creat__7A672E12");

            entity.HasOne(d => d.Standard).WithMany(p => p.CategoryStandards)
                .HasForeignKey(d => d.StandardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CategoryS__Stand__787EE5A0");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CategoryStandardUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__CategoryS__Updat__7B5B524B");
        });

        modelBuilder.Entity<CategoryStandardsKeyWord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC077C678085");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.KeyWord).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.CategoryStandard).WithMany(p => p.CategoryStandardsKeyWords)
                .HasForeignKey(d => d.CategoryStandardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CategoryS__Categ__04E4BC85");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CategoryStandardsKeyWordCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__CategoryS__Creat__05D8E0BE");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CategoryStandardsKeyWordUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__CategoryS__Updat__06CD04F7");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cities__3214EC07E554E366");

            entity.HasIndex(e => e.NameAr, "UQ_Cities_Name_Ar").IsUnique();

            entity.HasIndex(e => e.NameEn, "UQ_Cities_Name_En").IsUnique();

            entity.Property(e => e.NameAr)
                .HasMaxLength(200)
                .HasColumnName("Name_Ar");
            entity.Property(e => e.NameEn)
                .HasMaxLength(200)
                .HasColumnName("Name_En");

            entity.HasOne(d => d.Gov).WithMany(p => p.Cities)
                .HasForeignKey(d => d.GovId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cities__GovId__4CA06362");
        });

        modelBuilder.Entity<ContactU>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ContactU__3214EC07B7F9C6CA");

            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC0715E0C82B");

            entity.ToTable("Feedback");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.FeedbackCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Feedback__Create__6C190EBB");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.FeedbackUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__Feedback__Update__6D0D32F4");

            entity.HasOne(d => d.UserIdToNavigation).WithMany(p => p.FeedbackUserIdToNavigations)
                .HasForeignKey(d => d.UserIdTo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__UserId__6B24EA82");
        });

        modelBuilder.Entity<Founder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Founders__3214EC070FEAA322");

            entity.HasIndex(e => e.UserId, "UQ__Founders__1788CC4D257E4087").IsUnique();

            entity.HasOne(d => d.User).WithOne(p => p.Founder)
                .HasForeignKey<Founder>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Founders__UserId__5FB337D6");
        });

        modelBuilder.Entity<Government>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Governme__3214EC0745F293D2");

            entity.HasIndex(e => e.NameAr, "UQ_Governments_Name_Ar").IsUnique();

            entity.HasIndex(e => e.NameEn, "UQ_Governments_Name_En").IsUnique();

            entity.Property(e => e.NameAr)
                .HasMaxLength(200)
                .HasColumnName("Name_Ar");
            entity.Property(e => e.NameEn)
                .HasMaxLength(200)
                .HasColumnName("Name_En");
        });

        modelBuilder.Entity<Investor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Investor__3214EC0780F28580");

            entity.HasIndex(e => e.UserId, "UQ__Investor__1788CC4D151790F5").IsUnique();

            entity.HasOne(d => d.User).WithOne(p => p.Investor)
                .HasForeignKey<Investor>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Investors__UserI__6383C8BA");
        });

        modelBuilder.Entity<InvestorContactRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Investor__3214EC076FDDB974");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Business).WithMany(p => p.InvestorContactRequests)
                .HasForeignKey(d => d.BusinessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvestorC__Busin__09A971A2");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InvestorContactRequestCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__InvestorC__Creat__0B91BA14");

            entity.HasOne(d => d.Investor).WithMany(p => p.InvestorContactRequests)
                .HasForeignKey(d => d.InvestorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvestorC__Inves__0A9D95DB");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.InvestorContactRequestUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__InvestorC__Updat__0C85DE4D");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC074D68EC79");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.NotificationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Notificat__Creat__6754599E");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.NotificationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__Notificat__Updat__68487DD7");

            entity.HasOne(d => d.UserIdToNavigation).WithMany(p => p.NotificationUserIdToNavigations)
                .HasForeignKey(d => d.UserIdTo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__UserI__66603565");
        });

        modelBuilder.Entity<Standard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Standard__3214EC07FD4E8146");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.FormQuestion).HasColumnName("Form_Question");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.StandardCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Standards__Creat__73BA3083");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.StandardUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__Standards__Updat__74AE54BC");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07DBCD22E3");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105346C2B56BA").IsUnique();

            entity.HasIndex(e => e.NationalId, "UQ__Users__E9AA32FA413F176B").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.BackIdPicPath).IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.FrontIdPicPath).IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.HashedPassword)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.NationalId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProfilePicPath).IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.City).WithMany(p => p.Users)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__Users__CityId__59FA5E80");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InverseCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Users__CreatedBy__5AEE82B9");

            entity.HasOne(d => d.Government).WithMany(p => p.Users)
                .HasForeignKey(d => d.GovernmentId)
                .HasConstraintName("FK__Users__Governmen__59063A47");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.InverseUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__Users__UpdatedBy__5BE2A6F2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
