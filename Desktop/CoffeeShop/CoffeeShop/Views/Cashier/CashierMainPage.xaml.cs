using CoffeeShop.models;
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
using System.Windows.Shapes;
using System.Windows.Threading;


namespace CoffeeShop.Views.Cashier
{
    /// <summary>
    /// Логика взаимодействия для CashierMainPage.xaml
    /// </summary>
    public partial class CashierMainPage : Page
    {
        private DispatcherTimer timer;
        private CoffeeShopEntities context;
        /// <summary>
        /// Конструктор для CashierMainPage. Инициализирует компоненты и загружает информацию о смене.
        /// </summary>
        public CashierMainPage()
        {
            InitializeComponent();
            FIO.Text = $"{CurrentUser.FIO}, {CurrentUser.EmployeeID}";
            LoadShiftInfo();
            StartTimer();
            SetOpenShiftDate();


        }
        /// <summary>
        /// Загружает информацию о текущей смене и отображает её на странице.
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
        /// Запускает таймер для обновления информации о времени.
        /// </summary>
        private void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        /// <summary>
        /// Обработчик события таймера. Обновляет информацию о текущем времени.
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            SetOpenShiftDate();
        }
        /// <summary>
        /// Устанавливает текущее время и дату.
        /// </summary>
        private void SetOpenShiftDate()
        {
            DateTime currentDateTime = DateTime.Now;
            string formattedDateTime = currentDateTime.ToString("dd.MM.yyyy HH:mm"); 
        }
        /// <summary>
        /// Обработчик нажатия кнопки для закрытия личной смены.
        /// </summary>
        private void ClosePersonalShift_Click(object sender, RoutedEventArgs e)
        {
            
        }
        /// <summary>
        /// Открывает страницу личного кабинета.
        /// </summary>
        private void PersonalPage_Click(object sender, RoutedEventArgs e)
        {
            PersonalPage personalpage = new PersonalPage();
            personalpage.ShowDialog();
        }
        /// <summary>
        /// Обработчик нажатия кнопки для открытия новой смены.
        /// </summary>
        private void OpenShift_Click(object sender, RoutedEventArgs e)
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
        /// Обработчик нажатия кнопки для закрытия текущей смены.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void CloseShift_Click(object sender, RoutedEventArgs e)
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
        /// Открывает окно поиска чека.
        /// </summary>
        private void SearchReceipt_Click(object sender, RoutedEventArgs e)
        {
            SearchReceiptWindow searchReceiptWindow = new SearchReceiptWindow();
            searchReceiptWindow.ShowDialog();
        }

        
    }
}
