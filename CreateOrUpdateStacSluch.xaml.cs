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
    /// Логика взаимодействия для CreateOrUpdateStacSluch.xaml
    /// </summary>
    public partial class CreateOrUpdateStacSluch : Window
    {
        public CreateOrUpdateStacSluch()
        {
            InitializeComponent();
        }

        private void ButtonDel_Копировать_Click(object sender, RoutedEventArgs e)
        {
            SearchPac searchPac = new SearchPac();
            searchPac.ShowDialog();
            return;
        }

        private void ButtonTrans_Click(object sender, RoutedEventArgs e)
        {
            TransfPac transfPac = new TransfPac();
            transfPac.ShowDialog();
            return;
        }
    }
}
