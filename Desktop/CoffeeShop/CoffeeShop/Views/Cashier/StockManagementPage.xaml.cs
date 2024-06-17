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

namespace CoffeeShop.Views.Cashier
{
    /// <summary>
    /// Логика взаимодействия для StockManagementPage.xaml
    /// </summary>
    public partial class StockManagementPage : Page
    {
        /// <summary>
        /// Инициализирует новый экземпляр StockManagementPage.
        /// </summary>
        public StockManagementPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Обработчик нажатия кнопки "Просмотр склада". Навигирует на страницу ViewStockPage.
        /// </summary>
        private void ViewStock_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ViewStockPage());
        }


        /// <summary>
        /// Обработчик нажатия кнопки "Принять поставку". Навигирует на страницу ReceiveSupplyPage.
        /// </summary>
        private void ReceiveSupply_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ReceiveSupplyPage());
        }
        /// <summary>
        /// Обработчик нажатия кнопки "Просмотр продаж". Навигирует на страницу ViewSalesPage.
        /// </summary>
        private void ViewSales_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ViewSalesPage());
        }
    }
}
