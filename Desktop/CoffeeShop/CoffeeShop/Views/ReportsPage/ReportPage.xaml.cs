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

namespace CoffeeShop.Views.ReportsPage
{
    /// <summary>
    /// Логика взаимодействия для ReportPage.xaml
    /// </summary>
    public partial class ReportPage : Page
    {
        private CoffeeShopEntities context;
        public ReportPage()
        {
            InitializeComponent();
            context = new CoffeeShopEntities();

        }

        private void RevenueReport_Click(object sender, RoutedEventArgs e)
        {
            ReportFrame.Navigate(new RevenueReportPage());
        }

        private void StockReport_Click(object sender, RoutedEventArgs e)
        {
            ReportFrame.Navigate(new StockReportPage());
        }

        private void SupplyReport_Click(object sender, RoutedEventArgs e)
        {
            ReportFrame.Navigate(new SupplyReportPage());
        }

        private void SaleReport_Click(object sender, RoutedEventArgs e)
        {
            ReportFrame.Navigate(new ViewSalesPage());
        }
    }
}
