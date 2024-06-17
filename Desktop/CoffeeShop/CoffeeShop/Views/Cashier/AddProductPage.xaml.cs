using CoffeeShop.Views.Admin.StockAdmin;
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
    /// Логика взаимодействия для AddProductPage.xaml
    /// </summary>
    public partial class AddProductPage : Page
    {
        private CoffeeShopEntities context;
        private List<string> categories;
        public AddProductPage()
        {
            InitializeComponent();
            context = new CoffeeShopEntities();
            LoadCategories();
        }

        /// <summary>
        /// Загружает категории продуктов из базы данных в комбобокс.
        /// </summary>
        private void LoadCategories()
        {
            var categories = context.Product.Select(p => p.Category).Distinct().ToList();
            CategoryComboBox.ItemsSource = categories;
        }
        /// <summary>
        /// Обработчик кнопки для добавления продукта.
        /// Выполняет проверку данных и добавляет продукт в базу данных.
        /// </summary>
        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем данные из текстовых полей
            string productName = ProductNameTextBox.Text.Trim();
            string category = CategoryComboBox.Text.Trim();
            string description = DescriptionTextBox.Text.Trim();
            string unitPriceText = UnitPriceTextBox.Text.Trim();

            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(productName) || string.IsNullOrWhiteSpace(category) || string.IsNullOrWhiteSpace(unitPriceText))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверка корректности ввода цены
            if (!decimal.TryParse(unitPriceText, out decimal unitPrice) || unitPrice <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректную цену.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверка на наличие уже существующего продукта с таким же названием и категорией
            bool productExists = context.Product.Any(p => p.Name == productName && p.Category == category);
            if (productExists)
            {
                MessageBox.Show("Продукт с таким названием и категорией уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Создаем новый продукт и добавляем его в базу данных
                var newProduct = new Product
                {
                    Name = productName,
                    Category = category,
                    UnitPrice = unitPrice,
                    Description = description
                };

                context.Product.Add(newProduct);
                context.SaveChanges();

                MessageBox.Show("Продукт успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Очищаем текстовые поля
                ProductNameTextBox.Text = string.Empty;
                CategoryComboBox.Text = string.Empty;
                UnitPriceTextBox.Text = string.Empty;
                DescriptionTextBox.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении продукта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Добавляет категорию
        /// </summary>
        private void CreateCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var newCategoryDialog = new NewCategoryDialog();
            if (newCategoryDialog.ShowDialog() == true)
            {
                string newCategory = newCategoryDialog.CategoryName;

                // Проверка на наличие уже существующей категории
                bool categoryExists = context.Product.Any(p => p.Category == newCategory);
                if (categoryExists)
                {
                    MessageBox.Show("Категория с таким названием уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Обновляем список категорий и источник данных для комбобокса
                if (categories == null)
                {
                    categories = new List<string>();
                }

                categories.Add(newCategory);
                CategoryComboBox.ItemsSource = null;
                CategoryComboBox.ItemsSource = categories;
                CategoryComboBox.SelectedItem = newCategory;
            }
        }
    }
}
