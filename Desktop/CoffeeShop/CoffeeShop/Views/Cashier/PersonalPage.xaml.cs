using CoffeeShop.models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
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
    /// Логика взаимодействия для PersonalPage.xaml
    /// </summary>
    public partial class PersonalPage : Window
    {
        private CoffeeShopEntities context;
        /// <summary>
        /// Конструктор для PersonalPage. Инициализирует компоненты и загружает данные сотрудника.
        /// </summary>
        public PersonalPage()
        {
            InitializeComponent();
            context = new CoffeeShopEntities();
            
            LoadEmployeeData();
        }
        /// <summary>
        /// Загружает данные текущего пользователя и отображает их на странице.
        /// </summary>
        private void LoadEmployeeData()
        {

            FIOText.Text = $"{CurrentUser.FIO}";
            PositionText.Text = $"{CurrentUser.Position}";
            PhoneText.Text = $"{CurrentUser.Phone}";

            var shifts = context.Shifts.Where(s => s.EmployeeID == CurrentUser.EmployeeID);

            ShiftCountText.Text = shifts.Count().ToString();

            var totalHours = shifts.Sum(s => DbFunctions.DiffHours(s.StartTime, s.EndTime) ?? 0); 
            TotalHoursText.Text = totalHours.ToString();

            var totalRevenue = shifts.Sum(s => s.Receipts.Sum(r => r.TotalAmount));
            TotalRevenueText.Text = totalRevenue.ToString("C");
        }
        /// <summary>
        /// Обработчик нажатия кнопки закрытия окна.
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
