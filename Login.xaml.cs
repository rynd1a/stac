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

namespace stac
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (loginTextBox.Text == "админ")
            {
                this.Hide();
                Admin admin = new Admin();
                admin.Show();
                this.Close();
            }
            else
            {
                this.Hide();
                Employee employee = new Employee();
                employee.Show();
                this.Close();
            }
            return;
        }

        private void loginTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            loginTextBox.Text = "";
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            pass.Content = "";
        }
    }
}
