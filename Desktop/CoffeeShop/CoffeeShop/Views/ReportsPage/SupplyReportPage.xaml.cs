using CoffeeShop.models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace CoffeeShop.Views.ReportsPage
{
    /// <summary>
    /// Логика взаимодействия для SupplyReportPage.xaml
    /// </summary>
    public partial class SupplyReportPage : Page
    {
        private CoffeeShopEntities context;
        /// <summary>
        /// Инициализирует новый экземпляр класса SupplyReportPage.
        /// </summary>
        public SupplyReportPage()
        {
            InitializeComponent();
            context = new CoffeeShopEntities();
        }
        /// <summary>
        /// Генерирует отчет о поставках за указанный период и отображает его в DataGrid.
        /// </summary>
        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            if (startDate.HasValue && endDate.HasValue)
            {
                var supplyData = context.Purchases
                    .Where(s => s.PurchaseDate >= startDate.Value && s. PurchaseDate <= endDate.Value)
                    .Select(s => new SupplyReportItem
                    {
                        SupplyDate = s.PurchaseDate,
                        ProductName = s.Product.Name,
                        Quantity = s.Quantity,
                        Price = s.Price,
                        SupplierName = s.Suppliers.Name 
                    }).ToList();

                SupplyDataGrid.ItemsSource = supplyData;
                TotalSupplyTextBlock.Text = supplyData.Sum(s => s.Quantity * s.Price).ToString("C");
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите период.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        /// <summary>
        /// Печатает отчет о поставках, формируя документ Word.
        /// </summary>
        private void PrintReport_Click(object sender, RoutedEventArgs e)
        {
            var supplyReport = SupplyDataGrid.ItemsSource as IEnumerable<SupplyReportItem>;
            if (supplyReport == null || !supplyReport.Any())
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
                    doc.InsertParagraph("Отчет о поставках")
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

                    var table = doc.AddTable(supplyReport.Count() + 1, 5);
                    table.Design = TableDesign.MediumList2Accent1;

                    table.Rows[0].Cells[0].Paragraphs[0].Append("Дата поставки").Bold();
                    table.Rows[0].Cells[1].Paragraphs[0].Append("Продукт").Bold();
                    table.Rows[0].Cells[2].Paragraphs[0].Append("Количество").Bold();
                    table.Rows[0].Cells[3].Paragraphs[0].Append("Цена").Bold();
                    table.Rows[0].Cells[4].Paragraphs[0].Append("Поставщик").Bold();

                    int rowIndex = 1;
                    decimal totalSupplySum = 0;
                    foreach (var item in supplyReport)
                    {
                        table.Rows[rowIndex].Cells[0].Paragraphs[0].Append(item.SupplyDate.ToShortDateString());
                        table.Rows[rowIndex].Cells[1].Paragraphs[0].Append(item.ProductName);
                        table.Rows[rowIndex].Cells[2].Paragraphs[0].Append(item.Quantity.ToString());
                        table.Rows[rowIndex].Cells[3].Paragraphs[0].Append(item.Price.ToString("C"));
                        table.Rows[rowIndex].Cells[4].Paragraphs[0].Append(item.SupplierName);
                        totalSupplySum += item.Quantity * item.Price;
                        rowIndex++;
                    }

                    doc.InsertTable(table);

                    doc.InsertParagraph($"Итоговая сумма поставок за период: {totalSupplySum:C}")
                        .FontSize(14)
                        .SpacingBefore(20)
                        .SpacingAfter(20);

                    // Добавление места для подписи подписывающего лица
                    doc.InsertParagraph("Подпись: /________/ Дата: /____/_____/20____г.")
                        .FontSize(14)
                        .SpacingAfter(10);

                    // Добавление изображения подписи
                    string signatureImagePath = @"C:\Users\Gigabyte\Pictures\PP01.11\images.png";
                    if (System.IO.File.Exists(signatureImagePath))
                    {
                        var image = doc.AddImage(signatureImagePath);
                        var picture = image.CreatePicture();
                        picture.Width = 70;
                        picture.Height = 35;

                        var paragraph = doc.InsertParagraph("Подпись: ").FontSize(14).SpacingAfter(10);
                        paragraph.AppendPicture(picture);
                        paragraph.Append($" Дата: {DateTime.Now.ToShortDateString()}");
                    }

                    doc.InsertParagraph("Примечания:")
                        .FontSize(14)
                        .SpacingAfter(5);
                    doc.InsertParagraph("Данный отчет сформирован автоматически и содержит информацию о поставках за указанный период.")
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
        /// Модель данных для отчета о поставках.
        /// </summary>
        public class SupplyReportItem
        {
            public DateTime SupplyDate { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public string SupplierName { get; set; } // New property for supplier name
        }
    }
}
