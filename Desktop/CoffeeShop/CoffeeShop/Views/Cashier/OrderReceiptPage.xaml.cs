using CoffeeShop.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace CoffeeShop.Views.Cashier
{
    /// <summary>
    /// Логика взаимодействия для OrderReceiptPage.xaml
    /// </summary>
    public partial class OrderReceiptPage : Page
    {
        public ObservableCollection<ReceiptItem> receiptItems;
        private CoffeeShopEntities context;
        private List<Product> productsTextBox;
        private List<Product> products;

        public OrderReceiptPage()
        {
            InitializeComponent();
            receiptItems = new ObservableCollection<ReceiptItem>();
            ReceiptDataGrid.ItemsSource = receiptItems;
            LoadCategories();
            LoadAllProducts();




        }

        //private void CheckCurrentShift()
        //{
        //    using (context = new CoffeeShopEntities())
        //    {
        //        var currentShift = context.Shifts.FirstOrDefault(s => s.EmployeeID == CurrentUser.EmployeeID && s.EndTime == null);
        //        if (currentShift != null)
        //        {
        //            ShiftManager.CurrentShiftID = currentShift.ShiftID;
        //            MessageBox.Show($"Текущая смена ID: {ShiftManager.CurrentShiftID}");
        //        }

        //    }
        //}


        /// <summary>
        /// Загружает категории продуктов в ComboBox.
        /// </summary>
        private void LoadCategories()
        {
            using (context = new CoffeeShopEntities())
            {
                var categories = context.Product.Select(p => p.Category).Distinct().ToList();
                categories.Insert(0, "Все категории"); 
                CategoryComboBox.ItemsSource = categories;
                 
            }
        }
        /// <summary>
        /// Обработчик события изменения выбранной категории. Загружает продукты выбранной категории.
        /// </summary>
        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryComboBox.SelectedItem != null)
            {
                string selectedCategory = CategoryComboBox.SelectedItem.ToString();
                if (selectedCategory == "Все категории")
                {
                    LoadAllProducts(); 
                }
                else
                {
                    LoadProducts(selectedCategory); // Загружаем продукты выбранной категории
                }
            }
        }

        /// <summary>
        /// Обработчик события изменения выбранного продукта. Добавляет продукт в чек или увеличивает его количество.
        /// </summary>
        private void LoadAllProducts()
        {
            using (context = new CoffeeShopEntities())
            {
                
                products = context.Product.ToList();
                ProductListBox.ItemsSource = products;
                ProductListBox.DisplayMemberPath = "Name";
                ProductListBox.SelectedValuePath = "CategoryID";
            }
        }
        /// <summary>
        /// Обработчик события изменения выбранного продукта. Добавляет продукт в чек или увеличивает его количество.
        /// </summary>
        private void LoadProducts(string category)
        {
            using (context = new CoffeeShopEntities())
            {
                var products = context.Product.Where(p => p.Category == category).Select(p => new { p.ProductID, p.Name }).ToList();
                ProductListBox.ItemsSource = products;
                ProductListBox.DisplayMemberPath = "Name";
                ProductListBox.SelectedValuePath = "ProductID";

                productsTextBox = context.Product.Where(p => p.Category == category).ToList();
            }
        }


        /// <summary>
        /// Обработчик события изменения выбранного продукта. Добавляет продукт в чек или увеличивает его количество.
        /// </summary>
        private void ProductListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductListBox.SelectedItem != null)
            {
                var selectedProduct = (dynamic)ProductListBox.SelectedItem;
                int productId = selectedProduct.ProductID;
                string productName = selectedProduct.Name;
                DisplayStockQuantity(productId);

                var existingItem = receiptItems.FirstOrDefault(item => item.ProductID == productId);
                if (existingItem != null)
                {
                    existingItem.Quantity += 1;
                }
                else
                {
                    using (context = new CoffeeShopEntities())
                    {
                        var product = context.Product.Find(productId);
                        if (product != null)
                        {
                            var receiptItem = new ReceiptItem
                            {
                                ProductID = productId,
                                ProductName = productName,
                                Quantity = 1,  // По умолчанию добавляем 1 единицу
                                Price = product.UnitPrice,

                            };
                            receiptItems.Add(receiptItem);
                        }
                    }
                }
                ReceiptDataGrid.Items.Refresh();
                UpdateTotalAmount();
            }
        }
        /// <summary>
        /// Обновляет общую сумму чека.
        /// </summary>
        private void UpdateTotalAmount()
        {
            decimal totalAmount = receiptItems.Sum(item => item.Total);
            TotalAmountTextBlock.Text = $"Итого: {totalAmount:C}";
        }

        /// <summary>
        /// Фильтрация поиска продуктов
        /// </summary>
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchTextBox.Text.ToLower();
            var filteredProducts = products.Where(p => p.Name.ToLower().Contains(searchText)).ToList();
            ProductListBox.ItemsSource = filteredProducts;
        }
        /// <summary>
        /// Обработчик события подтверждения чека. Создает чек в базе данных и сохраняет его.
        /// </summary>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {


            if (ShiftManager.CurrentShiftID == null)
            {
                MessageBox.Show("Нет открытой смены. Пожалуйста, откройте смену.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (context = new CoffeeShopEntities())
            {
                var shiftExists = context.Shifts.Any(s => s.ShiftID == ShiftManager.CurrentShiftID);
                if (!shiftExists)
                {
                    MessageBox.Show($"Смена с ID {ShiftManager.CurrentShiftID} не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!receiptItems.Any())
                {
                    MessageBox.Show("Чек пуст. Добавьте хотя бы один продукт.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                foreach (var item in receiptItems)
                {
                    if (item.Quantity <= 0)
                    {
                        MessageBox.Show($"Количество продукта '{item.ProductName}' должно быть больше нуля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (item.Price <= 0)
                    {
                        MessageBox.Show($"Цена продукта '{item.ProductName}' должна быть больше нуля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var stockItem = context.Stock.FirstOrDefault(s => s.ProductID == item.ProductID);
                    if (stockItem == null)
                    {
                        MessageBox.Show($"Продукт '{item.ProductName}' не найден на складе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (stockItem.Quantity < item.Quantity)
                    {
                        MessageBox.Show($"Недостаточное количество продукта: {item.ProductName}. Доступно: {stockItem.Quantity}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                var receipt = new Receipts
                {
                    ShiftID = ShiftManager.CurrentShiftID.Value,
                    ReceiptDate = DateTime.Now,
                    TotalAmount = receiptItems.Sum(item => item.Total),
                    IsPaid = false
                };

                context.Receipts.Add(receipt);
                context.SaveChanges();

                var salesManager = new SalesManager();

                foreach (var item in receiptItems)
                {
                    var receiptItem = new ReceiptItems
                    {
                        ReceiptID = receipt.ReceiptID,
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };
                    context.ReceiptItems.Add(receiptItem);

                    
                    var stockItem = context.Stock.FirstOrDefault(s => s.ProductID == item.ProductID);
                    if (stockItem != null)
                    {
                        stockItem.Quantity -= item.Quantity;
                    }

                    salesManager.ProcessSale(item.ProductID, item.Quantity, item.Price);
                }

                context.SaveChanges();
                MessageBox.Show("Чек успешно создан!");
                receiptItems.Clear();
                UpdateTotalAmount();
            }
        }
        /// <summary>
        /// Обработчик события отмены чека. Очищает чек.
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            receiptItems.Clear();
            UpdateTotalAmount();
        }
        /// <summary>
        /// Обработчик события просмотра текущих заказов. Загружает текущие заказы.
        /// </summary>
        private void ViewCurrentOrders_Click(object sender, RoutedEventArgs e)
        {
            using (context = new CoffeeShopEntities())
            {
                var currentOrders = context.Receipts
                    .Where(r => r.ReceiptDate >= DateTime.Today && r.ShiftID == ShiftManager.CurrentShiftID && r.IsPaid == false)
                    .Select(r => new { r.ReceiptID, r.ReceiptDate, r.TotalAmount })
                    .ToList();

                OrdersListBox.ItemsSource = currentOrders;
            }
        }
        /// <summary>
        /// Обработчик события просмотра закрытых заказов. Загружает закрытые заказы.
        /// </summary>
        private void ViewClosedOrders_Click(object sender, RoutedEventArgs e)
        {
            if (ShiftManager.CurrentShiftID == null)
            {
                MessageBox.Show("Нет открытой смены. Пожалуйста, откройте смену.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (context = new CoffeeShopEntities())
            {
                var closedOrders = context.Receipts
                    .Where(r => r.ShiftID == ShiftManager.CurrentShiftID && r.IsPaid == true)
                    .Select(r => new { r.ReceiptID, r.ReceiptDate, r.TotalAmount })
                    .ToList();

                OrdersListBox.ItemsSource = closedOrders;
            }
        }
        /// <summary>
        /// Обработчик события изменения выбранного заказа. Отображает детали выбранного заказа.
        /// </summary>
        private void OrdersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrdersListBox.SelectedItem != null)
            {
                var selectedOrder = (dynamic)OrdersListBox.SelectedItem;
                int receiptId = selectedOrder.ReceiptID;

                using (context = new CoffeeShopEntities())
                {
                    var receipt = context.Receipts.FirstOrDefault(r => r.ReceiptID == receiptId);
                    if (receipt != null)
                    {
                        var orderDetails = context.ReceiptItems
                            .Where(ri => ri.ReceiptID == receiptId)
                            .Select(ri => new { ri.Product.Name, ri.Quantity, ri.Price, Total = ri.Quantity * ri.Price })
                            .ToList();

                        string orderDetailsText = string.Join(Environment.NewLine, orderDetails.Select(od => $"Продукт: {od.Name}, Количество: {od.Quantity}, Цена: {od.Price:C}, Всего: {od.Total:C}"));

                        string statusText = receipt.IsPaid ? "Статус: Оплачен" : "Статус: Не оплачен";
                        string cashierInfo = $"Кассир: {CurrentUser.FIO}";
                        string creationDate = $"Дата и время создания: {receipt.ReceiptDate:dd.MM.yyyy HH:mm:ss}";
                        

                        MessageBox.Show($"Детали чека:\n{orderDetailsText}\n\n{statusText}\n{cashierInfo}\n{creationDate}", "Детали чека");
                    }
                    else
                    {
                        MessageBox.Show("Чек не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Обработчик события оплаты чека. Обновляет статус выбранного чека на "оплачен".
        /// </summary>
        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersListBox.SelectedItem != null)
            {
                var selectedOrder = (dynamic)OrdersListBox.SelectedItem;
                int receiptId = selectedOrder.ReceiptID;

                using (context = new CoffeeShopEntities())
                {
                    var receipt = context.Receipts.FirstOrDefault(r => r.ReceiptID == receiptId);
                    if (receipt != null)
                    {
                        receipt.IsPaid = true; 
                        context.SaveChanges();
                        MessageBox.Show($"Чек #{receiptId} оплачен.");
                    }
                    else
                    {
                        MessageBox.Show("Чек не найден.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите чек для оплаты.");
            }
        }
        /// <summary>
        /// Отображает количество товара на складе для выбранного продукта.
        /// </summary>
        /// <param name="productId">Идентификатор продукта для отображения количества на складе.</param>
        private void DisplayStockQuantity(int productId)
        {
            using (context = new CoffeeShopEntities())
            {
                var stock = context.Stock.FirstOrDefault(s => s.ProductID == productId);
                if (stock != null)
                {
                    StockQuantityTextBlock.Text = $"Остаток: {stock.Quantity}";
                }
                else
                {
                    StockQuantityTextBlock.Text = "Остаток: 0";
                }
            }
        }
        
        

    }
}
