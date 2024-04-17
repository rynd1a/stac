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
    /// Логика взаимодействия для Employee.xaml
    /// </summary>
    public partial class Employee : Window
    {
        public Employee()
        {
            InitializeComponent();
        }

        private void ButPac_Click(object sender, RoutedEventArgs e)
        {
            FrameAdmin.Content = new Pacients();
        }

        private void ButSluch_Click(object sender, RoutedEventArgs e)
        {
            FrameAdmin.Content = new StacSluch();
        }

        private void ButPlace_Click(object sender, RoutedEventArgs e)
        {
            FrameAdmin.Content = new BedPlaceAndRoom();
        }

        private void ButLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
            this.Close();
            return;
        }
    }
}
