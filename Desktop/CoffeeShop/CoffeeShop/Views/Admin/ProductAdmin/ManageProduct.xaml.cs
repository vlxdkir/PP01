using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace CoffeeShop.Views.Admin.ProductAdmin
{
    /// <summary>
    /// Логика взаимодействия для ManageProduct.xaml
    /// </summary>
    public partial class ManageProduct : Window
    {
        private CoffeeShopEntities context;
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<string> Categories { get; set; }
        /// <summary>
        /// Конструктор класса ManageProduct
        /// </summary>
        public ManageProduct()
        {
            InitializeComponent();
            context = new CoffeeShopEntities();
            Categories = new ObservableCollection<string>();
            LoadProducts();
            LoadCategories();
        }
        /// <summary>
        /// Загрузка продуктов из базы данных
        /// </summary>
        private void LoadProducts()
        {
            var products = context.Product.ToList();
            Products = new ObservableCollection<Product>(products);
            ProductsDataGrid.ItemsSource = Products;
        }
        /// <summary>
        /// Загрузка категорий из базы данных
        /// </summary>
        private void LoadCategories()
        {
            var categories = context.Product.Select(p => p.Category).Distinct().ToList();
            CategoryComboBox.ItemsSource = categories;
        }
        /// <summary>
        /// Обработка выбора продукта в DataGrid
        /// </summary>
        private void ProductsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem != null)
            {
                var selectedProduct = (Product)ProductsDataGrid.SelectedItem;
                ProductNameTextBox.Text = selectedProduct.Name;
                ProductPriceTextBox.Text = selectedProduct.UnitPrice.ToString();
                CategoryComboBox.SelectedItem = selectedProduct.Category;
                DescriptionTextBox.Text = selectedProduct.Description;

                UpdateProductButton.IsEnabled = true;
                DeleteProductButton.IsEnabled = true;
            }
            else
            {
                ClearForm();
            }
        }
        /// <summary>
        /// Добавление нового продукта
        /// </summary>
        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                var newProduct = new Product
                {
                    Name = ProductNameTextBox.Text,
                    UnitPrice = decimal.Parse(ProductPriceTextBox.Text),
                    Category = CategoryComboBox.SelectedItem.ToString(),
                    Description = DescriptionTextBox.Text
                };

                context.Product.Add(newProduct);
                context.SaveChanges();
                Products.Add(newProduct);
                ClearForm();
            }
        }
        /// <summary>
        /// Обновление выбранного продукта
        /// </summary>
        private void UpdateProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem != null && ValidateInput())
            {
                var selectedProduct = (Product)ProductsDataGrid.SelectedItem;
                selectedProduct.Name = ProductNameTextBox.Text;
                selectedProduct.UnitPrice = decimal.Parse(ProductPriceTextBox.Text);
                selectedProduct.Category = CategoryComboBox.SelectedItem.ToString();
                selectedProduct.Description = DescriptionTextBox.Text;

                context.SaveChanges();
                ProductsDataGrid.Items.Refresh();
                ClearForm();
            }
        }
        /// <summary>
        /// Удаление выбранного продукта
        /// </summary>
        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem != null)
            {
                var selectedProduct = (Product)ProductsDataGrid.SelectedItem;
                context.Product.Remove(selectedProduct);
                context.SaveChanges();
                Products.Remove(selectedProduct);
                ClearForm();
            }
        }
        
        /// <summary>
        /// Очистка формы ввода
        /// </summary>
        private void ClearForm()
        {
            ProductNameTextBox.Clear();
            ProductPriceTextBox.Clear();
            CategoryComboBox.SelectedIndex = -1;
            DescriptionTextBox.Clear();
            UpdateProductButton.IsEnabled = false;
            DeleteProductButton.IsEnabled = false;
        }
        /// <summary>
        /// Валидация ввода
        /// </summary>
        /// <returns></returns>
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(ProductNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(ProductPriceTextBox.Text) ||
                CategoryComboBox.SelectedItem == null ||
                string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(ProductPriceTextBox.Text, out _))
            {
                MessageBox.Show("Некорректное значение цены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
        /// <summary>
        /// Закрытие окна
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
