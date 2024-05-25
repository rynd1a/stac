using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
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
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        public Admin()
        {
            InitializeComponent();
        }

        private void ButUser_Click(object sender, RoutedEventArgs e)
        {
            FrameAdmin.Content = new Users();
        }

        private void ButDepart_Click(object sender, RoutedEventArgs e)
        {
            FrameAdmin.Content = new Departs();
        }

        private void ButMedic_Click(object sender, RoutedEventArgs e)
        {
            FrameAdmin.Content = new Medics();
        }

        private void ButReport_Click(object sender, RoutedEventArgs e)
        {
            FrameAdmin.Content = new Reports();
        }

        private void ButLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
            this.Close();
            return;
        }

        private void ButPal_Click(object sender, RoutedEventArgs e)
        {
            FrameAdmin.Content = new BedPlaceAndRoom();
        }
    }
}
