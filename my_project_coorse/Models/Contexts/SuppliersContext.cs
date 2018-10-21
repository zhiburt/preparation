using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace preparation.Models.Contexts
{
    public class SuppliersContext : DbContext
    {
        public DbSet<preparation.Models.DbEntity.Supplier> Suppliers { get; set; }

        public SuppliersContext( DbContextOptions<SuppliersContext> options) : base(options)
        {
         
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<preparation.Models.DbEntity.Supplier>().ToTable("Suppliers");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
