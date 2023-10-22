using Microsoft.EntityFrameworkCore;

namespace ShoppingCartExercise.DatabaseContext
{
    public class ShoppingCartDatabaseContext : DbContext
    {

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
