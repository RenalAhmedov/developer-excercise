using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShopWeb.Core.Constants
{
    public static class OutputConstants
    {
        public const string EMPTY_BASKET = "Your shopping basket was empty.";
        public const string INVALID_PRODUCT = "Invalid product entered: {0}";
        public const string TWO_FOR_THREE_DEAL = "You received {0} for FREE because of the {1} deal!";
        public const string HALF_PRICE_DEAL = "You receive every second {0} for half price because of the {1} deal!";
        public const string TOTAL_COST = "Total cost: {0} aws ({1} clouds)";
    }
}
