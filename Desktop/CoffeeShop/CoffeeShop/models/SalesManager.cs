using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
namespace CoffeeShop.models
{
    public class SalesManager
    {
        private CoffeeShopEntities context;

        public SalesManager()
        {
            context = new CoffeeShopEntities();
        }

        // Учет продажи
        public void ProcessSale(int productId, int quantity, decimal price)
        {
            var sale = new Sales
            {
                ProductID = productId,
                Quantity = quantity,
                SaleDate = DateTime.Now,
                Price = price
            };
            context.Sales.Add(sale);

            var stock = context.Stock.FirstOrDefault(s => s.ProductID == productId);
            if (stock != null)
            {
                stock.Quantity -= quantity;
                stock.LastUpdate = DateTime.Now;
            }
            else
            {
                throw new Exception("Товара нет на складе");
            }

            context.SaveChanges();
        }
    }
}
