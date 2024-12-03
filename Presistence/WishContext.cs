using Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Presistence
{
    public class WishContext : DbContext
    {
        public DbSet<WishModel> Wishs => Set<WishModel>();

        public WishContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = Wish.db");
        }
    }
}
