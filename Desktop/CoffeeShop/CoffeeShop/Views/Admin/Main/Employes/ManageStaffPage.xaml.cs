using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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

namespace CoffeeShop.Views.Admin.Main.Employes
{
    /// <summary>
    /// Логика взаимодействия для ManageStaffPage.xaml
    /// </summary>
    public partial class ManageStaffPage : Window
    {
        private CoffeeShopEntities context;
        private Employees selectedEmployee;
        public ManageStaffPage()
        {
            InitializeComponent();
            context = new CoffeeShopEntities();
            LoadStaff();
            LoadPositions();
        }

        /// <summary>
        /// Загружает список сотрудников из базы данных.
        /// </summary>
        private void LoadStaff()
        {
            try
            {
                StaffDataGrid.ItemsSource = context.Employees.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Загружает список должностей.
        /// </summary>
        private void LoadPositions()
        {
            try
            {
                PositionComboBox.ItemsSource = new List<string> { "Администратор", "Кассир", "Бармен" };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке должностей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик события изменения выбора в таблице сотрудников.
        /// </summary>
        private void StaffDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StaffDataGrid.SelectedItem != null)
            {
                selectedEmployee = (Employees)StaffDataGrid.SelectedItem;
                FIOTextBox.Text = selectedEmployee.FIO;
                PhoneTextBox.Text = selectedEmployee.Phone;
                PositionComboBox.SelectedItem = selectedEmployee.Position;
                PinCodeTextBox.Text = selectedEmployee.Pincode;

                UpdateStaffButton.IsEnabled = true;
                DeleteStaffButton.IsEnabled = true;
            }
            else
            {
                ClearForm();
            }
        }

        /// <summary>
        /// Добавляет нового сотрудника в базу данных.
        /// </summary>
        private void AddStaffButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FIOTextBox.Text) || string.IsNullOrWhiteSpace(PhoneTextBox.Text) || PositionComboBox.SelectedItem == null || string.IsNullOrWhiteSpace(PinCodeTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newEmployee = new Employees
            {
                FIO = FIOTextBox.Text,
                Phone = PhoneTextBox.Text,
                Position = PositionComboBox.SelectedItem.ToString(),
                Pincode = PinCodeTextBox.Text
            };

            context.Employees.Add(newEmployee);
            context.SaveChanges();
            LoadStaff();
            ClearForm();
        }

        /// <summary>
        /// Обновляет данные выбранного сотрудника.
        /// </summary>
        private void UpdateStaffButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedEmployee != null)
            {
                try
                {
                    selectedEmployee.FIO = FIOTextBox.Text;
                    selectedEmployee.Phone = PhoneTextBox.Text;
                    selectedEmployee.Position = PositionComboBox.SelectedItem.ToString();
                    selectedEmployee.Pincode = PinCodeTextBox.Text;

                    context.SaveChanges();
                    LoadStaff();
                    ClearForm();
                }
                catch (DbEntityValidationException ex)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (var failure in ex.EntityValidationErrors)
                    {
                        sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                        foreach (var error in failure.ValidationErrors)
                        {
                            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                            sb.AppendLine();
                        }
                    }

                    MessageBox.Show($"Ошибка при обновлении сотрудника: {sb.ToString()}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Удаляет выбранного сотрудника из базы данных.
        /// </summary>
        private void DeleteStaffButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedEmployee != null)
            {
                try
                {
                    context.Employees.Remove(selectedEmployee);
                    context.SaveChanges();
                    LoadStaff();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Очищает форму и сбрасывает состояние кнопок.
        /// </summary>
        private void ClearForm()
        {
            FIOTextBox.Clear();
            PhoneTextBox.Clear();
            PositionComboBox.SelectedItem = null;
            PinCodeTextBox.Clear();
            UpdateStaffButton.IsEnabled = false;
            DeleteStaffButton.IsEnabled = false;
            selectedEmployee = null;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
