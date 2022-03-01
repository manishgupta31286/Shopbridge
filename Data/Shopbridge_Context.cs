using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shopbridge_base.Domain.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Shopbridge_base.Data
{
    public class Shopbridge_Context : DbContext
    {
        public Shopbridge_Context (DbContextOptions<Shopbridge_Context> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasPostgresExtension("uuid-ossp");
            builder.Entity<Product>()
                .HasIndex(p => p.ProductUpc)
                .IsUnique();
        }

        public DbSet<Product> Product { get; set; }
    }
}
