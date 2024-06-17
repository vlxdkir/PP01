using CoffeeShop.Views.Cashier;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CoffeeShop
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CashierMainPage mainWindow = new CashierMainPage();
            mainWindow.InitializeComponent();
        }

        protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
        {
            base.OnSessionEnding(e);

            using (var context = new CoffeeShopEntities())
            {
                var unpaidOrders = context.Receipts
                    .Where(r => !r.IsPaid)
                    .ToList();

                if (unpaidOrders.Any())
                {
                    MessageBoxResult result = MessageBox.Show("Есть неоплаченные заказы. Вы уверены, что хотите выйти?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.No)
                    {
                        e.Cancel = true; 
                    }
                }
            }
        }
    }
}
