using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.models
{
    public class RecipeCard
    {
        public int RecipeCardID { get; set; }
        public int ProductID { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
    }
}
