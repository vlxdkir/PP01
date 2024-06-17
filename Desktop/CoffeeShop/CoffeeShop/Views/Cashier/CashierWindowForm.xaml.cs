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

namespace CoffeeShop.Views.Cashier
{
    /// <summary>
    /// Логика взаимодействия для CashierWindowForm.xaml
    /// </summary>
    public partial class CashierWindowForm : Window
    {
        private DispatcherTimer timer;

        /// <summary>
        /// Конструктор CashierWindowForm. Инициализирует компоненты и запускает таймер.
        /// </summary>
        public CashierWindowForm()
        {
            InitializeComponent();
            MainFrame.Navigate(new CashierMainPage());
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

        }
        /// <summary>
        /// Обработчик события тика таймера. Обновляет текущее время и дату.
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Обновление текущего времени и даты
            CurrentTimeTextBlock.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }
        /// <summary>
        /// Обработчик нажатия кнопки для перехода на главную страницу кассира.
        /// </summary>
        private void MainPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CashierMainPage());
        }
        /// <summary>
        /// Обработчик нажатия кнопки для просмотра закрытых заказов.
        /// Возвращает пользователя на форму логина.
        /// </summary>
        private void ViewClosedOrders_Click(object sender, RoutedEventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();

            
            Window.GetWindow(this).Close();
        }
        /// <summary>
        /// Обработчик нажатия кнопки для перехода на страницу OrderReceiptPage.
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OrderReceiptPage());
        }
        /// <summary>
        /// Обработчик нажатия кнопки для перехода на страницу TehCardPage.
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TehCardPage());
        }
        /// <summary>
        /// Обработчик нажатия кнопки для перехода на страницу StockManagementPage.
        /// </summary>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new StockManagementPage());
        }
        /// <summary>
        /// Обработчик нажатия кнопки для перехода на страницу ReportPage.
        /// </summary>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ReportsPage.ReportPage());
        }
    }
}
