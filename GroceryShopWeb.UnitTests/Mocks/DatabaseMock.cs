using GroceryShopWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShopWeb.UnitTests.Mocks
{
    public class DatabaseMock
    {
        public static GroceryShopDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<GroceryShopDbContext>()
                    .UseInMemoryDatabase("GroceryShopUnitTestsDb"
                        + DateTime.Now.Ticks.ToString())
                    .Options;

                return new GroceryShopDbContext(dbContextOptions);
            }
        }
    }
}
