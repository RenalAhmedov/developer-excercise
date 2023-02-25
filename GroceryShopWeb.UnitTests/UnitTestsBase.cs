using GroceryShopWeb.Infrastructure.Data;
using GroceryShopWeb.UnitTests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShopWeb.UnitTests
{
    public class UnitTestsBase
    {
        protected GroceryShopDbContext context;

        [OneTimeSetUp]
        public void SetUpBase()
        {
            context = DatabaseMock.Instance;
        }
    }
}
