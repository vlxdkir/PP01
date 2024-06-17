using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.models
{
    public class ReceiptItem : INotifyPropertyChanged
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }

        private int quantity;
        public int Quantity
        {
            get => quantity;
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged("Quantity");
                    OnPropertyChanged("Total");
                }
            }
        }

        public decimal Price { get; set; }

        public decimal Total => Quantity * Price;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
