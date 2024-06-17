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
    /// Логика взаимодействия для ViewSalesPage.xaml
    /// </summary>
    public partial class ViewSalesPage : Page
    {
        private CoffeeShopEntities context;
        /// <summary>
        /// Инициализирует новый экземпляр ViewSalesPage.
        /// </summary>
        public ViewSalesPage()
        {
            InitializeComponent();
            context = new CoffeeShopEntities();
            LoadProducts();
        }
        /// <summary>
        /// Загружает список продуктов из базы данных и добавляет их в ComboBox.
        /// </summary>
        private void LoadProducts()
        {
            var products = context.Product.Select(p => new { p.ProductID, p.Name }).ToList();
            ProductComboBox.ItemsSource = products;
            ProductComboBox.DisplayMemberPath = "Name";
            ProductComboBox.SelectedValuePath = "ProductID";
        }
        /// <summary>
        /// Обработчик изменения выбора продукта в ComboBox.
        /// Загружает данные о продажах для выбранного продукта.
        /// </summary>
        private void ProductComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductComboBox.SelectedItem != null)
            {
                int selectedProductId = (int)ProductComboBox.SelectedValue;
                LoadSalesData(selectedProductId);
            }
        }
        /// <summary>
        /// Обработчик нажатия на кнопку применения фильтра по дате.
        /// Загружает данные о продажах для выбранного продукта с учетом даты.
        /// </summary>
        private void ApplyDateFilter_Click(object sender, RoutedEventArgs e)
        {
            if (ProductComboBox.SelectedItem != null)
            {
                int selectedProductId = (int)ProductComboBox.SelectedValue;
                LoadSalesData(selectedProductId);
            }
        }
        /// <summary>
        /// Загружает данные о продажах для указанного продукта с учетом выбранного периода.
        /// </summary>
        /// <param name="productId">ID выбранного продукта.</param>
        private void LoadSalesData(int productId)
        {
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            var salesQuery = context.Sales.Where(s => s.ProductID == productId);

            if (startDate.HasValue)
            {
                salesQuery = salesQuery.Where(s => s.SaleDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                salesQuery = salesQuery.Where(s => s.SaleDate <= endDate.Value);
            }

            var salesData = salesQuery
                .Select(s => new SaleViewModel
                {
                    SaleDate = s.SaleDate,
                    Quantity = s.Quantity,
                    Price = s.Price,
                    Total = s.Quantity * s.Price
                }).ToList();

            SalesDataGrid.ItemsSource = salesData;

            UpdateTotals(salesData);
        }
        /// <summary>
        /// Обновляет общие значения (количество и сумму) по данным о продажах.
        /// </summary>
        /// <param name="salesData">Список данных о продажах.</param>
        private void UpdateTotals(IList<SaleViewModel> salesData)
        {
            int totalQuantity = salesData.Sum(s => s.Quantity);
            decimal totalAmount = salesData.Sum(s => s.Total);

            TotalQuantityTextBlock.Text = totalQuantity.ToString();
            TotalAmountTextBlock.Text = totalAmount.ToString("C");
        }
        /// <summary>
        /// Представляет данные о продаже для отображения в DataGrid.
        /// </summary>
        public class SaleViewModel
        {
            public DateTime SaleDate { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal Total { get; set; }
        }
    }
}

