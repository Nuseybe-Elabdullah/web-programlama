using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using _1projedeneme.Models;
namespace _1projedeneme.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<_1projedeneme.Models.Salon> Salon { get; set; } = default!;
        public DbSet<_1projedeneme.Models.Antrenor> Antrenorler { get; set; }
        public DbSet<_1projedeneme.Models.Hizmet> Hizmetler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
        public DbSet<_1projedeneme.Models.Uye> Uyeler { get; set; }

    }
}