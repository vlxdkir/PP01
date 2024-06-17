using CoffeeShop.Views.Cashier;
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
using System.Windows.Threading;

namespace CoffeeShop.Views.Admin.Main
{
    /// <summary>
    /// Логика взаимодействия для AdminMainWindow.xaml
    /// </summary>
    public partial class AdminMainWindow : Window
    {
        private DispatcherTimer timer;
        /// <summary>
        /// Инициализирует новый экземпляр AdminMainWindow.
        /// </summary>
        public AdminMainWindow()
        {
            InitializeComponent();
            MainAdminFrame.Navigate(new AdminMainPage());
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        /// <summary>
        /// Обработчик события тика таймера. Обновляет текущие время и дату.
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Обновление текущего времени и даты
            CurrentTimeTextBlock.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }





        /// <summary>
        /// Обработчик кнопки для перехода на главную страницу администратора.
        /// </summary>
        private void MainPage_Click(object sender, RoutedEventArgs e)
        {
            MainAdminFrame.Navigate(new AdminMainPage());
        }
        /// <summary>
        /// Обработчик кнопки для перехода на страницу заказов кассира.
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainAdminFrame.Navigate(new Cashier.OrderReceiptPage());
        }
        /// <summary>
        /// Обработчик кнопки для перехода на страницу технологических карт.
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainAdminFrame.Navigate(new Cashier.TehCardPage());
        }
        /// <summary>
        /// Обработчик кнопки для перехода на страницу отчетов.
        /// </summary>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainAdminFrame.Navigate(new ReportsPage.ReportPage());
        }
        /// <summary>
        /// Обработчик кнопки для перехода на страницу управления складом.
        /// </summary>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainAdminFrame.Navigate(new StockAdmin.StockAdminPage());
        }
        /// <summary>
        /// Обработчик кнопки для выхода из учетной записи администратора и перехода на форму логина.
        /// </summary>
        private void ViewClosedOrders_Click(object sender, RoutedEventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();

            // Закрыть текущую форму
            Window.GetWindow(this).Close();
        }
    }
}
