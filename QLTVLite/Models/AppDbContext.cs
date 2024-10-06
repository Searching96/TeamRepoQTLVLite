using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTVLite.Models
{
    internal class AppDbContext : DbContext
    {     
    
    public DbSet<Sach> SACH { get; set; }
    public DbSet<TacGia> TACGIA { get; set; }
    public DbSet<Sach_TacGia> SACH_TACGIA { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=LAPTOP-S9VDQ6Q1;Database=QLTVLite;Trusted_Connection=True;TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình bảng liên kết SACH_TACGIA
            modelBuilder.Entity<Sach_TacGia>()
                .HasKey(stg => new { stg.IDSach, stg.IDTacGia });

            modelBuilder.Entity<Sach_TacGia>()
                .HasOne(stg => stg.Sach)
                .WithMany(s => s.Sach_TacGia)
                .HasForeignKey(stg => stg.IDSach);

            modelBuilder.Entity<Sach_TacGia>()
                .HasOne(stg => stg.TacGia)
                .WithMany(tg => tg.Sach_TacGia)
                .HasForeignKey(stg => stg.IDTacGia);
        }
    }
}
