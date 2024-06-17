using CoffeeShop.models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using Xceed.Document.NET;
using Xceed.Words.NET;


namespace CoffeeShop.Views.Cashier
{
    /// <summary>
    /// Логика взаимодействия для SearchReceiptWindow.xaml
    /// </summary>
    public partial class SearchReceiptWindow : Window
    {
        private CoffeeShopEntities context;
        /// <summary>
        /// Конструктор для SearchReceiptWindow. Инициализирует компоненты и контекст данных.
        /// </summary>
        public SearchReceiptWindow()
        {
            InitializeComponent();
            context = new CoffeeShopEntities();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Найти чек". Ищет чек по номеру и отображает информацию о нем.
        /// </summary>
        private void FindReceipt_Click(object sender, RoutedEventArgs e)
        {
            string receiptNumber = ReceiptNumberTextBox.Text;
            if (string.IsNullOrEmpty(receiptNumber))
            {
                MessageBox.Show("Пожалуйста, введите номер чека.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var receipt = context.Receipts
                .Where(r => r.ReceiptID.ToString() == receiptNumber)
                .Select(r => new
                {
                    r.ReceiptID,
                    r.ReceiptDate,
                    CashierName = r.Shifts.Employees.FIO,
                    Items = r.ReceiptItems.Select(ri => new
                    {
                        ProductName = ri.Product.Name,
                        Quantity = ri.Quantity,
                        Price = ri.Price,
                        Total = ri.Quantity * ri.Price
                    }).ToList(),
                    TotalAmount = r.ReceiptItems.Sum(ri => ri.Quantity * ri.Price)
                })
                .FirstOrDefault();

            if (receipt == null)
            {
                MessageBox.Show("Чек не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ReceiptInfoPanel.Visibility = Visibility.Visible;
            ReceiptNumberText.Text = "Номер чека: " + receipt.ReceiptID;
            ReceiptDateText.Text = "Дата создания: " + receipt.ReceiptDate.ToString("g");
            ReceiptCashierText.Text = "Кассир: " + receipt.CashierName;
            ReceiptItemsListBox.ItemsSource = receipt.Items.Select(i => $"{i.ProductName} x {i.Quantity} @ {i.Price:C} = {i.Total:C}").ToList();
            ReceiptTotalText.Text = "Итоговая сумма: " + receipt.TotalAmount.ToString("C");

            
        }
        /// <summary>
        /// Сохраняет чек в файл Word.
        /// </summary>
        /// <param name="receiptNumber">Номер чека.</param>
        private void SaveReceiptToFile(string receiptNumber)
        {
            string directoryPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Receipts");
            Directory.CreateDirectory(directoryPath); // Создание папки, если её нет

            // Удаляем недопустимые символы из имени файла
            string validFileName = $"Receipt_{receiptNumber}.docx".Replace("\\", "").Replace("/", "").Replace(":", "").Replace("*", "").Replace("?", "").Replace("\"", "").Replace("<", "").Replace(">", "").Replace("|", "");

            string filePath = System.IO.Path.Combine(directoryPath, validFileName);

            using (var doc = DocX.Create(filePath))
            {
                // Заголовок
                doc.InsertParagraph("Кофейня \"Sunrise\"")
                    .FontSize(20)
                    .Bold()
                    .Alignment = Alignment.center;

                // Информация об ИП
                doc.InsertParagraph("ИП Кириллов В.Ю.")
                    .FontSize(14)
                    .Alignment = Alignment.center;

                doc.InsertParagraph("ИНН: 1234567890")
                    .FontSize(12)
                    .Alignment = Alignment.center;

                doc.InsertParagraph("ОГРН: 1234567890123")
                    .FontSize(12)
                    .Alignment = Alignment.center;

                doc.InsertParagraph("Адрес: г. Москва, ул. Примерная, д. 1")
                    .FontSize(12)
                    .Alignment = Alignment.center;

                doc.InsertParagraph("Телефон: +7 (123) 456-78-90")
                    .FontSize(12)
                    .Alignment = Alignment.center;

                doc.InsertParagraph("")
                    .FontSize(14)
                    .SpacingAfter(20);

                // Информация о чеке
                doc.InsertParagraph($"{receiptNumber}")
                    .FontSize(14)
                    .SpacingAfter(5);

                doc.InsertParagraph($"{ReceiptDateText.Text}")
                    .FontSize(14)
                    .SpacingAfter(5);

                doc.InsertParagraph($"{ReceiptCashierText.Text}")
                    .FontSize(14)
                    .SpacingAfter(20);

                // Состав чека
                doc.InsertParagraph("")
                    .FontSize(14)
                    .SpacingAfter(5);

                var table = doc.AddTable(ReceiptItemsListBox.Items.Count + 1, 4);
                table.Design = TableDesign.LightListAccent1;

                table.Rows[0].Cells[0].Paragraphs[0].Append("Продукт").Bold().FontSize(12);
                table.Rows[0].Cells[1].Paragraphs[0].Append("Количество").Bold().FontSize(12);
                table.Rows[0].Cells[2].Paragraphs[0].Append("Цена").Bold().FontSize(12);
                table.Rows[0].Cells[3].Paragraphs[0].Append("Всего").Bold().FontSize(12);

                int rowIndex = 1;
                foreach (var item in ReceiptItemsListBox.Items)
                {
                    var parts = item.ToString().Split(new[] { " x ", " @ ", " = " }, StringSplitOptions.None);
                    table.Rows[rowIndex].Cells[0].Paragraphs[0].Append(parts[0]).FontSize(12);
                    table.Rows[rowIndex].Cells[1].Paragraphs[0].Append(parts[1]).FontSize(12);
                    table.Rows[rowIndex].Cells[2].Paragraphs[0].Append(parts[2]).FontSize(12);
                    table.Rows[rowIndex].Cells[3].Paragraphs[0].Append(parts[3]).FontSize(12);
                    rowIndex++;
                }

                doc.InsertTable(table);

                // Итоговая сумма
                doc.InsertParagraph($"{ReceiptTotalText.Text}")
                    .FontSize(18)
                    .Bold()
                    .SpacingBefore(20)
                    .Alignment = Alignment.right;

                doc.InsertParagraph("--------------------------------------------------------------")
                    .FontSize(14)
                    .SpacingAfter(20)
                    .Alignment = Alignment.right;

                // Благодарственное сообщение
                doc.InsertParagraph("Спасибо за покупку!")
                    .FontSize(16)
                    .Bold()
                    .Alignment = Alignment.center;

                doc.Save();
            }

            MessageBox.Show($"Чек успешно сохранен в {filePath}.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        /// <summary>
        /// Обработчик нажатия кнопки "Печать чека". Сохраняет чек в файл Word.
        /// </summary>
        private void PrintReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (ReceiptInfoPanel.Visibility == Visibility.Collapsed)
            {
                MessageBox.Show("Сначала найдите чек.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveReceiptToFile(ReceiptNumberText.Text);
        }
    }
    
}
