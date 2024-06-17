using CoffeeShop.Views.Cashier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoffeeShop.Views.Admin.StockAdmin
{
    /// <summary>
    /// Логика взаимодействия для StockAdminPage.xaml
    /// </summary>
    public partial class StockAdminPage : Page
    {
        public StockAdminPage()
        {
            InitializeComponent();
        }

        private void ViewStock_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ViewStockPage());
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AddProductPage());
        }

        private void ReceiveSupply_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ReceiveSupplyPage());
        }
    }
}
