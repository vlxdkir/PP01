//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoffeeShop
{
    using System;
    using System.Collections.Generic;
    
    public partial class Purchases
    {
        public int PurchaseID { get; set; }
        public int SupplierID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public System.DateTime PurchaseDate { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual Suppliers Suppliers { get; set; }
    }
}
