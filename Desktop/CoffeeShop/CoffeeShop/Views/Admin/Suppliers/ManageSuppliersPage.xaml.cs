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
using CoffeeShop.models;

namespace CoffeeShop.Views.Admin.Suppliers
{
    /// <summary>
    /// Логика взаимодействия для ManageSuppliersPage.xaml
    /// </summary>
    public partial class ManageSuppliersPage : Window
    {
        private CoffeeShopEntities context;
        private CoffeeShop.Suppliers selectedSupplier;

        /// <summary>
        /// Инициализирует новый экземпляр класса ManageSuppliersPage.
        /// </summary>
        public ManageSuppliersPage()
        {
            InitializeComponent();
            context = new CoffeeShopEntities();
            LoadSuppliers();
        }

        /// <summary>
        /// Загружает список поставщиков из базы данных.
        /// </summary>
        private void LoadSuppliers()
        {
            SuppliersDataGrid.ItemsSource = context.Suppliers.ToList();
        }
        /// <summary>
        /// Обработчик события изменения выбора в таблице поставщиков.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void SuppliersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SuppliersDataGrid.SelectedItem != null)
            {
                selectedSupplier = (CoffeeShop.Suppliers)SuppliersDataGrid.SelectedItem;
                SupplierNameTextBox.Text = selectedSupplier.Name;
                SupplierContactTextBox.Text = selectedSupplier.Contact;

                UpdateSupplierButton.IsEnabled = true;
                DeleteSupplierButton.IsEnabled = true;
            }
            else
            {
                ClearForm();
            }
        }
        /// <summary>
        /// Добавляет нового поставщика в базу данных.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void AddSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SupplierNameTextBox.Text) || string.IsNullOrWhiteSpace(SupplierContactTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newSupplier = new CoffeeShop.Suppliers
            {
                Name = SupplierNameTextBox.Text,
                Contact = SupplierContactTextBox.Text
            };

            context.Suppliers.Add(newSupplier);
            context.SaveChanges();
            LoadSuppliers();
            ClearForm();
        }
        /// <summary>
        /// Обновляет данные выбранного поставщика.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void UpdateSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSupplier != null)
            {
                selectedSupplier.Name = SupplierNameTextBox.Text;
                selectedSupplier.Contact = SupplierContactTextBox.Text;

                context.SaveChanges();
                LoadSuppliers();
                ClearForm();
            }
        }
        /// <summary>
        /// Удаляет выбранного поставщика из базы данных.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void DeleteSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSupplier != null)
            {
                context.Suppliers.Remove(selectedSupplier);
                context.SaveChanges();
                LoadSuppliers();
                ClearForm();
            }
        }
        /// <summary>
        /// Очищает форму и сбрасывает состояние кнопок.
        /// </summary>
        private void ClearForm()
        {
            SupplierNameTextBox.Clear();
            SupplierContactTextBox.Clear();
            UpdateSupplierButton.IsEnabled = false;
            DeleteSupplierButton.IsEnabled = false;
            selectedSupplier = null;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
