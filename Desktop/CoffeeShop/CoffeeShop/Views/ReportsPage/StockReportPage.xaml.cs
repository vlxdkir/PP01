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
    /// Логика взаимодействия для OrderReceiptPage.xaml
    /// </summary>
    public partial class StockReportPage : Page
    {
        /// <summary>
        /// Логика взаимодействия для StockReportPage.xaml
        /// </summary>
        private CoffeeShopEntities context;

        /// <summary>
        /// Инициализирует новый экземпляр класса StockReportPage.
        /// </summary>
        public StockReportPage()
        {
            InitializeComponent();
            context = new CoffeeShopEntities();
            
        }
        /// <summary>
        /// Загружает данные о состоянии склада за указанный период.
        /// </summary>
        private void LoadStockData(object sender, RoutedEventArgs e)
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
                var stockData = context.Stock
                    .Select(s => new StockReportItem
                    {
                        ProductID = s.ProductID,
                        ProductName = s.Product.Name,
                        Quantity = s.Quantity,
                        SoldQuantity = context.Sales
                            .Where(sale => sale.ProductID == s.ProductID && sale.SaleDate >= startDate && sale.SaleDate <= endDate)
                            .Sum(sale => (int?)sale.Quantity) ?? 0,
                        LastUpdate = s.LastUpdate,
                        ProductDescription = s.Product.Description,
                        ProductPrice = s.Product.UnitPrice
                    }).ToList();

                if (!stockData.Any())
                {
                    MessageBox.Show("Нет данных за выбранный период.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                StockDataGrid.ItemsSource = stockData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Печатает отчет о состоянии склада, формируя документ Word.
        /// </summary>
        private void PrintReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var stockReport = StockDataGrid.ItemsSource as IEnumerable<StockReportItem>;
                if (stockReport == null || !stockReport.Any())
                {
                    MessageBox.Show("Нет данных для печати.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DateTime? startDate = StartDatePicker.SelectedDate;
                DateTime? endDate = EndDatePicker.SelectedDate;

                if (!startDate.HasValue || !endDate.HasValue || startDate.Value > endDate.Value)
                {
                    MessageBox.Show("Пожалуйста, выберите корректный период.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                        doc.InsertParagraph("Отчет о состоянии склада")
                            .FontSize(20)
                            .Bold()
                            .Alignment = Alignment.center;

                        doc.InsertParagraph($"Период: {startDate.Value.ToShortDateString()} - {endDate.Value.ToShortDateString()}")
                            .FontSize(14)
                            .SpacingAfter(5);

                        doc.InsertParagraph($"Дата формирования отчета: {DateTime.Now.ToShortDateString()}")
                            .FontSize(14)
                            .SpacingAfter(5);

                        doc.InsertParagraph($"Кассир: {CurrentUser.FIO}")
                            .FontSize(14)
                            .SpacingAfter(20);

                        var table = doc.AddTable(stockReport.Count() + 1, 4);
                        table.Design = TableDesign.MediumList2Accent1;

                        table.Rows[0].Cells[0].Paragraphs[0].Append("Продукт").Bold();
                        table.Rows[0].Cells[1].Paragraphs[0].Append("Количество на складе").Bold();
                        table.Rows[0].Cells[2].Paragraphs[0].Append("Количество продано").Bold();
                        table.Rows[0].Cells[3].Paragraphs[0].Append("Последнее обновление").Bold();

                        int rowIndex = 1;
                        int totalSoldQuantity = 0;
                        foreach (var item in stockReport)
                        {
                            table.Rows[rowIndex].Cells[0].Paragraphs[0].Append(item.ProductName);
                            table.Rows[rowIndex].Cells[1].Paragraphs[0].Append(item.Quantity.ToString());
                            table.Rows[rowIndex].Cells[2].Paragraphs[0].Append(item.SoldQuantity.ToString());
                            table.Rows[rowIndex].Cells[3].Paragraphs[0].Append(item.LastUpdate.ToShortDateString());
                            totalSoldQuantity += item.SoldQuantity;
                            rowIndex++;
                        }

                        doc.InsertTable(table);

                        doc.InsertParagraph($"Итоговое количество проданных продуктов за период: {totalSoldQuantity}")
                            .FontSize(14)
                            .SpacingBefore(20)
                            .SpacingAfter(20);

                        // Добавление места для подписи подписывающего лица
                        doc.InsertParagraph("Подпись /________/ Дата /____/_____/20____г.")
                            .FontSize(14)
                            .SpacingAfter(10);

                        // Добавление изображения подписи
                        string signatureImagePath = @"C:\Users\Gigabyte\Pictures\PP01.11\images.png";
                        var image = doc.AddImage(signatureImagePath);
                        var picture = image.CreatePicture();
                        picture.Width = 70;
                        picture.Height = 35;

                        

                        doc.InsertParagraph("Примечания:")
                            .FontSize(14)
                            .SpacingAfter(5);
                        doc.InsertParagraph("Данный отчет сформирован автоматически и содержит информацию о состоянии склада за указанный период.")
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
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при создании отчета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Класс, представляющий элемент отчета о состоянии склада.
        /// </summary>

        private void StockDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            var selectedCell = dataGrid.CurrentCell;
            var selectedItem = selectedCell.Item as StockReportItem;

            if (selectedItem != null)
            {
                string columnName = selectedCell.Column.Header.ToString();
                switch (columnName)
                {
                    case "Продукт":
                        MessageBox.Show($"Описание: {selectedItem.ProductDescription}\nЦена: {selectedItem.ProductPrice:C}", "Информация о продукте", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Количество на складе":
                        MessageBox.Show($"Текущий запас: {selectedItem.Quantity}", "Информация о количестве", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Количество продано":
                        var sales = context.Sales
                            .Where(sale => sale.ProductID == selectedItem.ProductID && sale.SaleDate >= StartDatePicker.SelectedDate && sale.SaleDate <= EndDatePicker.SelectedDate)
                            .Select(sale => new
                            {
                                sale.Quantity,
                                sale.SaleDate,
                                
                            }).ToList();

                        var salesInfo = string.Join("\n", sales.Select(sale => $"Количество: {sale.Quantity}, Дата: {sale.SaleDate}"));
                        MessageBox.Show(salesInfo, "Информация о продажах", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Последнее обновление":
                        MessageBox.Show($"Последнее обновление: {selectedItem.LastUpdate}", "Информация об обновлении", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                }
            }
        }
        /// <summary>
        /// Класс, представляющий элемент отчета о состоянии склада.
        /// </summary>
        public class StockReportItem
        {
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public int SoldQuantity { get; set; }
            public DateTime LastUpdate { get; set; }
            public string ProductDescription { get; set; }
            public decimal ProductPrice { get; set; }
            public int ProductID { get; set; }
        }

        
    }
}
