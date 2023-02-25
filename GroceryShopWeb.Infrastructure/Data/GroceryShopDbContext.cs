using GroceryShopWeb.Infrastructure.Data.Entities;
using GroceryShopWeb.Infrastructure.DataConstants;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GroceryShopWeb.Infrastructure.Data
{
    public class GroceryShopDbContext : IdentityDbContext
    {
        public GroceryShopDbContext(DbContextOptions<GroceryShopDbContext> options)
            : base(options)
        {
            if (Database.IsRelational())
            {
                Database.Migrate();
            }
            else
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Deal> Deals { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            this.SeedProducts(builder);
            this.SeedDeals(builder);

            base.OnModelCreating(builder);
        }

        private void SeedProducts(ModelBuilder builder)
        {
            var products = new List<Product>()
            {
                new Product()
                {
                    Id = 1,
                    Name = "apple",
                    Price = 50
                },
                new Product()
                {
                    Id = 2,
                    Name = "banana",
                    Price = 40
                },
                new Product()
                {
                    Id = 3,
                    Name = "tomato",
                    Price = 30
                },
                new Product()
                {
                    Id = 4,
                    Name = "potato",
                    Price = 26
                }
            };

            builder.Entity<Product>().HasData(products);
        }

        private void SeedDeals(ModelBuilder builder)
        {
            var deals = new List<Deal>()
            {
                new Deal()
                {
                    Id = 1,
                    Type = DealConstants.TwoForThree,
                    Arguments = "apple, banana, tomato"
                },
                new Deal()
                {
                    Id = 2,
                    Type = DealConstants.HalfPrice,
                    Arguments = "potato"
                }
            };

            builder.Entity<Deal>().HasData(deals);
        }
    }
}