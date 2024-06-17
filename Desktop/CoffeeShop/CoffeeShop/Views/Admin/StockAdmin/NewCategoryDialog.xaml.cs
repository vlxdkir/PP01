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

namespace CoffeeShop.Views.Admin.StockAdmin
{
    /// <summary>
    /// Логика взаимодействия для NewCategoryDialog.xaml
    /// </summary>
    public partial class NewCategoryDialog : Window
    {
        public string CategoryName { get; private set; }
        public NewCategoryDialog()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Ввод данных для добавления
        /// </summary>
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            string categoryName = CategoryNameTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                MessageBox.Show("Пожалуйста, введите название категории.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CategoryName = categoryName;
            DialogResult = true;
            Close();
        }
        /// <summary>
        /// Закрытие окна
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
