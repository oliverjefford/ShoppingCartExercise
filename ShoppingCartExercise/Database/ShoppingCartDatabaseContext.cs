using Microsoft.EntityFrameworkCore;
using ShoppingCartExercise.Models.DatabaseModels;

namespace ShoppingCartExercise.DatabaseContext
{
    public class ShoppingCartDatabaseContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Offer> Offers { get; set; }

        public ShoppingCartDatabaseContext()
        {
        }

        public ShoppingCartDatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Name=ConnectionStrings:ShoppingCartDatabase");
        }
    }
}
