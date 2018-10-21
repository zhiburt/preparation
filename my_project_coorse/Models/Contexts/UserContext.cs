using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using preparation.Models.Account;
using preparation.Services.Messenger;

namespace preparation.Models.Contexts
{
    public class UserContext : IdentityDbContext<User>
    {
        public DbSet<Messenger> MessangessDbSet;
        public DbSet<Supplier> SuppliersDbSet;

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<User>()
            //    .HasMany(c => c.Messages)
            //    .WithOne(m => m.From);
            //.HasForeignKey((m1) => m1.Id);

        }
    }
}
