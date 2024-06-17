using CoffeeShop.models;
using CoffeeShop.Utillits;
using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace CoffeeShop.Views.Cashier
{
    /// <summary>
    /// Логика взаимодействия для AddRecipeDialog.xaml
    /// </summary>
    public partial class AddRecipeDialog : Window
    {
        private CoffeeShopEntities context;
        private byte[] imageBytes;
        /// <summary>
        /// Конструктор AddRecipeDialog. Инициализирует компоненты и загружает продукты.
        /// </summary>
        public AddRecipeDialog()
        {
            InitializeComponent();
            context = new CoffeeShopEntities();
            LoadProducts();

        }
        /// <summary>
        /// Загружает список продуктов из базы данных и устанавливает его в ProductComboBox.
        /// </summary>
        private void LoadProducts()
        {
            try
            {
                var products = context.Product.ToList();
                ProductComboBox.ItemsSource = products;
                ProductComboBox.DisplayMemberPath = "Name";
                ProductComboBox.SelectedValuePath = "ProductID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке продуктов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Обработчик нажатия кнопки выбора изображения. Открывает диалог выбора файла и загружает изображение.
        /// </summary>
        private void ChooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

                if (openFileDialog.ShowDialog() == true)
                {
                    imageBytes = new ImageManipulation().ConvertImageToByteArray(openFileDialog.FileName);
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openFileDialog.FileName);
                    bitmap.EndInit();
                    SelectedImage.Source = bitmap;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Обработчик нажатия кнопки сохранения. Проверяет корректность введенных данных и сохраняет новый рецепт в базу данных.
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ProductComboBox.SelectedItem != null && !string.IsNullOrEmpty(DescriptionTextBox.Text) && imageBytes != null)
                {
                    var recipeCard = new RecipeCards
                    {
                        ProductID = (int)ProductComboBox.SelectedValue,
                        Description = DescriptionTextBox.Text,
                        Image = imageBytes
                    };

                    context.RecipeCards.Add(recipeCard);
                    context.SaveChanges();
                    MessageBox.Show("Рецепт успешно добавлен!");
                    this.DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, заполните все поля и выберите изображение.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении рецепта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Обработчик нажатия кнопки отмены. Закрывает диалог без сохранения.
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
