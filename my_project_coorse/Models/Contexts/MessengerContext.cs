using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using preparation.Models.DbEntity;

namespace preparation.Models.Contexts
{
    public class MessengerContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }

        public MessengerContext(DbContextOptions<MessengerContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().ToTable("Messages");
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
