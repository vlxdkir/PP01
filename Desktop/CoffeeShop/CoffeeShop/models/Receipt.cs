using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.models
{
    public class Receipts
    {
        public int ReceiptID { get; set; }
        public int ShiftID { get; set; }
        public DateTime ReceiptDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; } // Новое свойство
    }
}
