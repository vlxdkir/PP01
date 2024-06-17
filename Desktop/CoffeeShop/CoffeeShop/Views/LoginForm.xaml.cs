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
using CoffeeShop.models;
using CoffeeShop.Views.Admin.Main;
using CoffeeShop.Views.Cashier;

namespace CoffeeShop.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {

        private StringBuilder pinCodeBuilder = new StringBuilder();
        
        public LoginForm()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                pinCodeBuilder.Append(button.Content.ToString());
                UpdatePinCodeDisplay();
                if (pinCodeBuilder.Length > 3)
                {
                    CheckPinCode(pinCodeBuilder.ToString());
                }
                
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            pinCodeBuilder.Clear();
            UpdatePinCodeDisplay();
            MessageBox.Show("Пин-код очищен");
        }

        public bool CheckPinCode(string enteredPinCode)
        {
            if (PincodeValidator.ValidatePincode(enteredPinCode, out string errorMessage))
            {
                using (var context = new CoffeeShopEntities())
                {
                    var user = context.Employees.FirstOrDefault(u => u.Pincode == enteredPinCode);

                    if (user != null)
                    {
                        CurrentUser.EmployeeID = user.EmployeeID;
                        CurrentUser.FIO = user.FIO;
                        CurrentUser.Position = user.Position;
                        CurrentUser.Phone = user.Phone;
                        CurrentUser.Pincode = user.Pincode;

                        if (user.Position == "Администратор")
                        {
                            AdminMainWindow adminMainWindow = new AdminMainWindow();
                            adminMainWindow.Show();
                        }
                        else
                        {
                            CashierWindowForm cashierWindowForm = new CashierWindowForm();
                            cashierWindowForm.Show();
                        }

                        this.Close();
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Неверный ПИН-код. Попробуйте снова.");
                        pinCodeBuilder.Clear();
                        UpdatePinCodeDisplay();
                        return false;
                    }
                }
            }
            else
            {
                MessageBox.Show(errorMessage);
                pinCodeBuilder.Clear();
                UpdatePinCodeDisplay();
                return false;
            }
        }


        private void UpdatePinCodeDisplay()
        {
            PinCodeDisplay.Text = new string('*', pinCodeBuilder.Length);
        }










        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btn_maximize_Click(object sender, RoutedEventArgs e)
        {
            SwitchWindowState();
        }

        private void btn_minimize_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow()
        {
            Window.GetWindow(this).WindowState = WindowState.Maximized;
        }

        private void RestoreWindow()
        {
            Window.GetWindow(this).WindowState = WindowState.Normal;
        }

        private void SwitchWindowState()
        {
            if (Window.GetWindow(this).WindowState == WindowState.Normal) MaximizeWindow();
            else RestoreWindow();
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                SwitchWindowState();
                return;
            }

            if (Window.GetWindow(this).WindowState == WindowState.Maximized)
            {
                return;
            }
            else
            {
                if (e.LeftButton == MouseButtonState.Pressed) Window.GetWindow(this).DragMove();
            }
        }

        private void MinimizeAndDragMove(MouseButtonEventArgs e)
        {
            if (Window.GetWindow(this).WindowState == WindowState.Maximized)
            {
                double percentHorizontal = e.GetPosition(this).X / ActualWidth;
                double targetHorizontal = Window.GetWindow(this).RestoreBounds.Width * percentHorizontal;

                double percentVertical = e.GetPosition(this).Y / ActualHeight;
                double targetVertical = Window.GetWindow(this).RestoreBounds.Height * percentVertical;

                Window.GetWindow(this).WindowStyle = WindowStyle.None;
                RestoreWindow();

                var mousePosition = e.GetPosition(this);

                Window.GetWindow(this).Left = mousePosition.X - targetHorizontal;
                Window.GetWindow(this).Top = mousePosition.Y - targetVertical;
            }

            if (e.LeftButton == MouseButtonState.Pressed) Window.GetWindow(this).DragMove();
            Window.GetWindow(this).WindowStyle = WindowStyle.SingleBorderWindow;
        }

        public void WindowStateChanged(WindowState state)
        {
            if (Window.GetWindow(this).WindowState == WindowState.Maximized)
            {
                btn_maximize.Content = "\uE923";
                titleBar.Height = 24;
            }
            else if (Window.GetWindow(this).WindowState == WindowState.Normal)
            {
                btn_maximize.Content = "\uE922";
                titleBar.Height = 32;
            }
        }
    }
}
