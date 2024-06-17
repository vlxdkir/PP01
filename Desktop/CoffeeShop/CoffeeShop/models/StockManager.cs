using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.models
{
    public class StockManager
    {
        private CoffeeShopEntities context;

        public StockManager()
        {
            context = new CoffeeShopEntities();
        }

        // Прием товара
        public void ReceiveStock(int supplierId, int productId, int quantity, decimal price)
        {
            var purchase = new Purchases
            {
                SupplierID = supplierId,
                ProductID = productId,
                Quantity = quantity,
                PurchaseDate = DateTime.Now,
                Price = price,
                Status = "Получено"
            };
            context.Purchases.Add(purchase);

            var stock = context.Stock.FirstOrDefault(s => s.ProductID == productId);
            if (stock != null)
            {
                stock.Quantity += quantity;
                stock.LastUpdate = DateTime.Now;
            }
            else
            {
                stock = new Stock
                {
                    ProductID = productId,
                    Quantity = quantity,
                    LastUpdate = DateTime.Now
                };
                context.Stock.Add(stock);
            }

            context.SaveChanges();
        }

        public List<Stock> GetStock()
        {
            return context.Stock.ToList();
        }
    }
}
