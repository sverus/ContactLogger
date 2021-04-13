using ContactLogger.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactLogger.Data
{
    public class ContactContext : DbContext
    {
        private readonly IConfiguration _config;

        public ContactContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<ContactLog> ContactLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("ContactLogger"));
        }

        protected override void OnModelCreating(ModelBuilder bldr)
        {
            bldr.Entity<Student>();
            bldr.Entity<ContactLog>();

        }
    }
}
