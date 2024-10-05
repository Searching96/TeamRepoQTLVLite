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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=LAPTOP-S9VDQ6Q1;Database=QLTVLite;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}
