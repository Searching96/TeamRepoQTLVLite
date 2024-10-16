using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Library_DTO;

public partial class LibraryManagementContext : DbContext
{
    public LibraryManagementContext()
    {
    }

    public LibraryManagementContext(DbContextOptions<LibraryManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Borrow> Borrows { get; set; }

    public virtual DbSet<BorrowDetail> BorrowDetails { get; set; }

    public virtual DbSet<Reader> Readers { get; set; }

    public virtual DbSet<ReaderType> ReaderTypes { get; set; }

    public virtual DbSet<Return> Returns { get; set; }

    public virtual DbSet<ReturnDetail> ReturnDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-LTO7VK3;Database=LibraryManagement;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PK_Admin");

            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.UsernameNavigation).WithOne(p => p.Admin)
                .HasForeignKey<Admin>(d => d.Username)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Username_Admin");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK_Book");

            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.IsBorrowed)
                .HasDefaultValueSql("((0))")
                .HasColumnName("isBorrowed");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('')");
        });

        modelBuilder.Entity<Borrow>(entity =>
        {
            entity.HasKey(e => e.BorrowId).HasName("PK_Borrow");

            entity.Property(e => e.BorrowId).HasColumnName("BorrowID");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.UsernameNavigation).WithMany(p => p.Borrows)
                .HasForeignKey(d => d.Username)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserID_Borrow");
        });

        modelBuilder.Entity<BorrowDetail>(entity =>
        {
            entity.HasKey(e => new { e.BorrowId, e.BookId }).HasName("PK_BorrowDetail");

            entity.Property(e => e.BorrowId).HasColumnName("BorrowID");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.EndDate)
                .HasDefaultValueSql("(getdate()+(30))")
                .HasColumnType("date");

            entity.HasOne(d => d.Book).WithMany(p => p.BorrowDetails)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookID_Borrow");
        });

        modelBuilder.Entity<ReaderType>(entity =>
        {
            entity.HasKey(e => e.ReaderTypeId).HasName("PK_ReaderType");

            entity.Property(e => e.ReaderTypeId).HasColumnName("ReaderTypeID");
            entity.Property(e => e.ReaderTypeName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PK_Reader");

            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CurrentBorrows).HasDefaultValueSql("((0))");
            entity.Property(e => e.EndDate)
                .HasDefaultValueSql("(getdate()+(365))")
                .HasColumnType("date");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.ReaderTypeId)
                .HasDefaultValueSql("((1))")
                .HasColumnName("ReaderTypeID");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.TotalDebt).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.ReaderType).WithMany(p => p.Readers)
                .HasForeignKey(d => d.ReaderTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReaderType");

            entity.HasOne(d => d.UsernameNavigation).WithOne(p => p.Reader)
                .HasForeignKey<Reader>(d => d.Username)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Username_Reader");
        });

        modelBuilder.Entity<Return>(entity =>
        {
            entity.HasKey(e => e.ReturnId).HasName("PK_Return");

            entity.Property(e => e.ReturnId).HasColumnName("ReturnID");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.UsernameNavigation).WithMany(p => p.Returns)
                .HasForeignKey(d => d.Username)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserID_Return");
        });

        modelBuilder.Entity<ReturnDetail>(entity =>
        {
            entity.HasKey(e => new { e.ReturnId, e.BookId }).HasName("PK_ReturnDetail");

            entity.Property(e => e.ReturnId).HasColumnName("ReturnID");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.Note)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ReturnDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");

            entity.HasOne(d => d.Book).WithMany(p => p.ReturnDetails)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookID_Return");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PK_User");

            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TypeOfUser)
                .HasMaxLength(10)
                .HasDefaultValueSql("Reader")
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
