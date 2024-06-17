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
using static CoffeeShop.Views.ReportsPage.StockReportPage;
using LiveCharts;
using LiveCharts.Wpf;

namespace CoffeeShop.Views.Cashier
{
    /// <summary>
    /// Логика взаимодействия для ViewStockPage.xaml
    /// </summary>
    public partial class ViewStockPage : Page
    {
        private CoffeeShopEntities context;
        private List<StockReportItem> stockData;
        public List<string> ProductLabels { get; set; }
        public SeriesCollection StockValues { get; set; }

        /// <summary>
        /// Конструктор для инициализации ViewStockPage и загрузки данных склада.
        /// </summary>
        public ViewStockPage()
        {
            InitializeComponent();
            context = new CoffeeShopEntities();
            DataContext = this;
            LoadStock();
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadStock();
        }
        /// <summary>
        /// Загружает данные склада из базы данных и устанавливает источник данных для DataGrid.
        /// </summary>
        private void LoadStock()
        {
            var stockData = context.Stock.Select(s => new StockReportItem
            {
                ProductName = s.Product.Name,
                Category = s.Product.Category,
                Quantity = s.Quantity,
                LastUpdate = s.LastUpdate
            }).ToList();

            StockDataGrid.ItemsSource = stockData;
        }
        /// <summary>
        /// Обработчик события изменения текста в поле фильтрации.
        /// Фильтрует данные в DataGrid на основе введенного текста.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = FilterTextBox.Text.ToLower();
            var filteredData = context.Stock.Where(s => s.Product.Name.ToLower().Contains(filterText)).Select(s => new StockReportItem
            {
                ProductName = s.Product.Name,
                Category = s.Product.Category,
                Quantity = s.Quantity,
                LastUpdate = s.LastUpdate
            }).ToList();

            StockDataGrid.ItemsSource = filteredData;
        }


        /// <summary>
        /// Класс для представления элемента отчета по складу.
        /// Содержит информацию о продукте, его категории, количестве и дате последнего обновления.
        /// </summary>
        public class StockReportItem
        {
            public string ProductName { get; set; }
            public string Category { get; set; }
            public int Quantity { get; set; }
            public System.DateTime LastUpdate { get; set; }
        }
    }
}
