using CoffeeShop.models;
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
    /// Логика взаимодействия для ReceiveSupplyPage.xaml
    /// </summary>
    public partial class ReceiveSupplyPage : Page
    {
        private CoffeeShopEntities context;
        private List<Product> products;
        private List<Suppliers> suppliers;
        private List<Product> allProducts;
        public ReceiveSupplyPage()
        {
            InitializeComponent();
            context = new CoffeeShopEntities();
            LoadSuppliers();
            LoadProducts();
        }
        /// <summary>
        /// Загружает поставщиков из базы данных и заполняет SupplierComboBox.
        /// </summary>
        private void LoadSuppliers()
        {
            var suppliers = context.Suppliers.Select(s => new { s.SupplierID, s.Name }).ToList();
            SupplierComboBox.ItemsSource = suppliers;
            SupplierComboBox.DisplayMemberPath = "Name";
            SupplierComboBox.SelectedValuePath = "SupplierID";
        }
        /// <summary>
        /// Загружает продукты из базы данных и заполняет ProductComboBox.
        /// </summary>
        private void LoadProducts()
        {
            allProducts = context.Product.ToList();
            ProductComboBox.ItemsSource = allProducts;
            ProductComboBox.DisplayMemberPath = "Name";
            ProductComboBox.SelectedValuePath = "ProductID";
        }
        
        
        /// <summary>
        /// Обработчик нажатия на кнопку "Принять поставку".
        /// </summary>
        private void ReceiveSupplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (SupplierComboBox.SelectedItem == null || ProductComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите поставщика и продукт.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректное количество.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректную цену.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var supplierId = (int)SupplierComboBox.SelectedValue;
            var productId = (int)ProductComboBox.SelectedValue;
            price = int.Parse(QuantityTextBox.Text);
            price = decimal.Parse(PriceTextBox.Text);

            var stockManager = new StockManager();
            stockManager.ReceiveStock(supplierId, productId, quantity, price);

            MessageBox.Show("Поставка успешно принята!");
            ClearFields();
        }
        /// <summary>
        /// Фильтрует combobox
        /// </summary>
        private void ProductComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(ProductComboBox.Text))
            {
                ProductComboBox.ItemsSource = allProducts;
                return;
            }

            string filterText = ProductComboBox.Text.ToLower();
            var filteredProducts = allProducts
                .Where(p => p.Name.ToLower().Contains(filterText))
                .ToList();

            ProductComboBox.ItemsSource = filteredProducts;
            ProductComboBox.IsDropDownOpen = true;
            ProductComboBox.Text = filterText;
            var textBox = (TextBox)ProductComboBox.Template.FindName("PART_EditableTextBox", ProductComboBox);
            if (textBox != null)
            {
                
                textBox.CaretIndex = filterText.Length;
            }
        }

        /// <summary>
        /// Очищает все поля ввода.
        /// </summary>
        private void ClearFields()
        {
            SupplierComboBox.SelectedIndex = -1;
            ProductComboBox.SelectedIndex = -1;
            QuantityTextBox.Clear();
            PriceTextBox.Clear();
        }
    }
}
