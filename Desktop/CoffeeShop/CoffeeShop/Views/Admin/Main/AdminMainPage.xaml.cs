using CoffeeShop.models;
using CoffeeShop.Views.Admin.Main.Employes;
using CoffeeShop.Views.Admin.StockAdmin;
using CoffeeShop.Views.Admin.Suppliers;
using CoffeeShop.Views.Cashier;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using System.Windows.Threading;

namespace CoffeeShop.Views.Admin.Main
{
    /// <summary>
    /// Логика взаимодействия для AdminMainPage.xaml
    /// </summary>
    public partial class AdminMainPage : Page
    {
        private DispatcherTimer timer;
        private CoffeeShopEntities context;
        /// <summary>
        /// Инициализирует новый экземпляр AdminMainPage.
        /// </summary>
        public AdminMainPage()
        {
            InitializeComponent();
            FIO.Text = $"{CurrentUser.FIO}, {CurrentUser.EmployeeID}";
            StartTimer();
            LoadShiftInfo();
        }

        /// <summary>
        /// Загружает информацию о текущей смене.
        /// </summary>
        private void LoadShiftInfo()
        {
            if (ShiftManager.CurrentShiftID.HasValue)
            {
                ShiftStatus.Text = $"Смена открыта {ShiftManager.ShiftStartTime:dd.MM.yyyy HH:mm:ss}";
                ShiftKassa.Text = $"Смена #{ShiftManager.CurrentShiftID} открыта {ShiftManager.ShiftStartTime:dd.MM.yyyy HH:mm:ss}";
                FIOKassa.Text = $"Менеджер: {ShiftManager.CashierName}";
            }
            else
            {
                ShiftStatus.Text = "Смена не открыта";
                ShiftKassa.Text = "Смена не открыта";
                FIOKassa.Text = "Менеджер не назначен";
            }
        }
        /// <summary>
        /// Запускает таймер для обновления времени.
        /// </summary>
        private void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        /// <summary>
        /// Обработчик события тика таймера. Обновляет дату и время.
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            SetOpenShiftDate();
        }
        /// <summary>
        /// Устанавливает текущую дату и время.
        /// </summary>
        private void SetOpenShiftDate()
        {
            DateTime currentDateTime = DateTime.Now;
            string formattedDateTime = currentDateTime.ToString("dd.MM.yyyy HH:mm");
        }
        /// <summary>
        /// Обработчик кнопки поиска чека. Открывает окно поиска чека.
        /// </summary>
        private void SearchAdminReceipt_Click(object sender, RoutedEventArgs e)
        {
            SearchReceiptWindow searchReceiptWindow = new SearchReceiptWindow();
            searchReceiptWindow.ShowDialog();
        }
        /// <summary>
        /// Обработчик кнопки открытия смены. Открывает новую смену.
        /// </summary>
        private void OpenAdminShift_Click(object sender, RoutedEventArgs e)
        {
            using (context = new CoffeeShopEntities())
            {
                var newShift = new Shifts
                {
                    EmployeeID = CurrentUser.EmployeeID,
                    StartTime = DateTime.Now
                };

                context.Shifts.Add(newShift);
                context.SaveChanges();
                ShiftManager.StartShift(newShift.ShiftID, CurrentUser.FIO, newShift.StartTime.Value);

                MessageBox.Show($"Смена открыта в {newShift.StartTime}, Менеджер: {CurrentUser.FIO}");
                LoadShiftInfo();
            }
        }
        /// <summary>
        /// Обработчик кнопки закрытия смены. Закрывает текущую смену.
        /// </summary>
        private void CloseAdminShift_Click(object sender, RoutedEventArgs e)
        {
            if (ShiftManager.CurrentShiftID == null)
            {
                MessageBox.Show("Нет открытой смены.");
                return;
            }

            using (context = new CoffeeShopEntities())
            {
                var shift = context.Shifts.FirstOrDefault(s => s.ShiftID == ShiftManager.CurrentShiftID);

                if (shift != null)
                {
                    shift.EndTime = DateTime.Now;
                    context.SaveChanges();

                    MessageBox.Show($"Смена #{ShiftManager.CurrentShiftID} закрыта в {shift.EndTime}");
                    ShiftManager.EndShift();
                    LoadShiftInfo();
                }
                else
                {
                    MessageBox.Show("Открытая смена не найдена.");
                }
            }
        }
        /// <summary>
        /// Обработчик кнопки закрытия личной смены.
        /// </summary>
        private void CloseAdminPersonalShift_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// Обработчик кнопки перехода на личную страницу. Открывает личную страницу.
        /// </summary>
        private void PersonalAdminPage_Click(object sender, RoutedEventArgs e)
        {
            PersonalPage personalpage = new PersonalPage();
            personalpage.ShowDialog();
        }
        /// <summary>
        /// Обработчик кнопки добавления поставщика. Открывает окно управления поставщиками.
        /// </summary>
        private void AddSupplier_Click(object sender, RoutedEventArgs e)
        {
            ManageSuppliersPage stockwindow = new ManageSuppliersPage();
            stockwindow.ShowDialog();
        }
        /// <summary>
        /// Обработчик кнопки управления продуктами.
        /// </summary>
        private void ManageProducts_Click(object sender, RoutedEventArgs e)
        {
            ProductAdmin.ManageProduct productwindow = new ProductAdmin.ManageProduct();
            productwindow.ShowDialog();
        }
        /// <summary>
        /// Обработчик кнопки управления сотрудниками. Открывает окно управления сотрудниками.
        /// </summary>
        private void Staff_Click(object sender, RoutedEventArgs e)
        {
            ManageStaffPage staffwindow = new ManageStaffPage();
            staffwindow.ShowDialog();
        }
    }
}
