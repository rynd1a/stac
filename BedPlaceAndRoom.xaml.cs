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

namespace stac
{
    /// <summary>
    /// Логика взаимодействия для BedPlaceAndRoom.xaml
    /// </summary>
    public partial class BedPlaceAndRoom : Page
    {
        public BedPlaceAndRoom()
        {
            InitializeComponent();
        }

        private void ButtonNewRoom_Click(object sender, RoutedEventArgs e)
        {
            CreateOrUpdateRoom createOrUpdateRoom = new CreateOrUpdateRoom();
            createOrUpdateRoom.ShowDialog();
            return;
        }

        private void ButtonNewPlace_Click(object sender, RoutedEventArgs e)
        {
            CreateOrUpdateBedPlace createOrUpdateBedPlace = new CreateOrUpdateBedPlace();
            createOrUpdateBedPlace.ShowDialog();
            return;
        }
    }
}
