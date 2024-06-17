using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using Xceed.Document.NET;
using Xceed.Words.NET;
using CoffeeShop.models;
using System.Data.Entity;

namespace CoffeeShop.Views.ReportsPage
{
    /// <summary>
    /// Логика взаимодействия для RevenueReportPage.xaml
    /// </summary>
    public partial class RevenueReportPage : Page
    {
        private CoffeeShopEntities context;

        /// <summary>
        /// Инициализирует новый экземпляр класса RevenueReportPage.
        /// </summary>
        public RevenueReportPage()
        {
            InitializeComponent();
            context = new CoffeeShopEntities();
        }
        /// <summary>
        /// Генерирует отчет о выручке за указанный период и отображает его в DataGrid.
        /// </summary>
        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            
            if (!startDate.HasValue || !endDate.HasValue)
            {
                MessageBox.Show("Пожалуйста, выберите период.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            if (startDate > endDate)
            {
                MessageBox.Show("Дата начала не может быть больше даты окончания.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                
                var receipts = context.Receipts
                    .Where(r => r.ReceiptDate >= startDate.Value && r.ReceiptDate <= endDate.Value && r.IsPaid)
                    .ToList();

                
                var revenueReport = receipts
                    .GroupBy(r => r.ReceiptDate.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        OrderCount = g.Count(),
                        TotalRevenue = g.Sum(r => r.TotalAmount)
                    }).ToList();

                if (!revenueReport.Any())
                {
                    MessageBox.Show("Нет данных за выбранный период.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                RevenueDataGrid.ItemsSource = revenueReport;
                TotalRevenueTextBlock.Text = revenueReport.Sum(r => r.TotalRevenue).ToString("C");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при генерации отчета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Печатает отчет о выручке, формируя документ Word.
        /// </summary>
        private void PrintReport_Click(object sender, RoutedEventArgs e)
        {
            var revenueReport = RevenueDataGrid.ItemsSource as IEnumerable<dynamic>;
            if (revenueReport == null || !revenueReport.Any())
            {
                MessageBox.Show("Нет данных для печати.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Document|*.docx",
                Title = "Сохранить отчет"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var doc = DocX.Create(saveFileDialog.FileName))
                {
                    doc.InsertParagraph("Отчет о выручке")
                        .FontSize(20)
                        .Bold()
                        .Alignment = Alignment.center;

                    doc.InsertParagraph($"Период: {StartDatePicker.SelectedDate.Value.ToShortDateString()} - {EndDatePicker.SelectedDate.Value.ToShortDateString()}")
                        .FontSize(14)
                        .SpacingAfter(5);

                    doc.InsertParagraph($"Дата формирования отчета: {DateTime.Now.ToShortDateString()}")
                        .FontSize(14)
                        .SpacingAfter(5);

                    doc.InsertParagraph($"Кассир: {CurrentUser.FIO}")
                        .FontSize(14)
                        .SpacingAfter(20);

                    var table = doc.AddTable(revenueReport.Count() + 1, 3);
                    table.Design = TableDesign.MediumList2Accent1;

                    table.Rows[0].Cells[0].Paragraphs[0].Append("Дата").Bold();
                    table.Rows[0].Cells[1].Paragraphs[0].Append("Количество заказов").Bold();
                    table.Rows[0].Cells[2].Paragraphs[0].Append("Общая выручка").Bold();

                    int rowIndex = 1;
                    foreach (var item in revenueReport)
                    {
                        table.Rows[rowIndex].Cells[0].Paragraphs[0].Append(item.Date.ToShortDateString());
                        table.Rows[rowIndex].Cells[1].Paragraphs[0].Append(((int)item.OrderCount).ToString());
                        table.Rows[rowIndex].Cells[2].Paragraphs[0].Append(((decimal)item.TotalRevenue).ToString("C"));
                        rowIndex++;
                    }

                    doc.InsertTable(table);

                    int totalOrders = revenueReport.Sum(r => (int)r.OrderCount);
                    decimal totalRevenue = revenueReport.Sum(r => (decimal)r.TotalRevenue);

                    doc.InsertParagraph($"Итоговое количество заказов: {totalOrders}")
                        .FontSize(14)
                        .SpacingBefore(20);

                    doc.InsertParagraph($"Итоговая выручка: {totalRevenue:C}")
                        .FontSize(14)
                        .SpacingAfter(20);

                    // Добавление места для подписи подписывающего лица
                    doc.InsertParagraph("Подпись: /__________/  Дата: /____/________/20____г.")
                        .FontSize(14)
                        .SpacingAfter(10);

                    // Добавление изображения подписи
                    string signatureImagePath = @"C:\Users\Gigabyte\Pictures\PP01.11\images.png"; 
                    var image = doc.AddImage(signatureImagePath);
                    var picture = image.CreatePicture();
                    picture.Width = 70; 
                    picture.Height = 35; 

                    var paragraph = doc.InsertParagraph("Подпись: ").FontSize(14).SpacingAfter(10);
                    paragraph.AppendPicture(picture);
                    paragraph.Append($"Дата: {DateTime.Now.ToShortDateString()}");

                    doc.InsertParagraph("Примечания:")
                        .FontSize(14)
                        .SpacingAfter(5);

                    doc.InsertParagraph("Данный отчет сформирован автоматически и содержит информацию о выручке за указанный период.")
                        .FontSize(12)
                        .SpacingAfter(5);

                    doc.InsertParagraph("Пожалуйста, свяжитесь с бухгалтерией для дополнительной информации.")
                        .FontSize(12)
                        .SpacingAfter(5);

                    doc.Save();
                }

                MessageBox.Show("Отчет успешно сохранен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
        }
        /// <summary>
        /// Просмотр данных
        /// </summary>
        private void RevenueDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RevenueDataGrid.SelectedItem != null)
            {
                var selectedDate = (DateTime)((dynamic)RevenueDataGrid.SelectedItem).Date;
                ShowOrdersForDate(selectedDate);
            }
        }
        /// <summary>
        /// Функция для выделения и фильтрации данных для просмотра данных
        /// </summary>
        private void ShowOrdersForDate(DateTime date)
        {
            var orders = context.Receipts
                .Where(r => DbFunctions.TruncateTime(r.ReceiptDate) == date)
                .Select(r => new
                {
                    r.ReceiptID,
                    r.ReceiptDate,
                    r.TotalAmount
                })
                .ToList();

            if (orders.Any())
            {
                StringBuilder ordersInfo = new StringBuilder();
                ordersInfo.AppendLine($"Заказы за {date:dd.MM.yyyy}:\n");

                foreach (var order in orders)
                {
                    ordersInfo.AppendLine($"Номер заказа: {order.ReceiptID}");
                    ordersInfo.AppendLine($"Дата: {order.ReceiptDate}");
                    ordersInfo.AppendLine($"Сумма: {order.TotalAmount:C}");
                    ordersInfo.AppendLine(new string('-', 30));
                }

                MessageBox.Show(ordersInfo.ToString(), "Заказы", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Заказов на выбранную дату нет.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
