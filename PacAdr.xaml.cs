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

namespace stac
{
    /// <summary>
    /// Логика взаимодействия для PacAdr.xaml
    /// </summary>
    public partial class PacAdr : Window
    {
        public PacAdr()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (CreateorUpdatePac.getCurrentAdrRowNumber() == -1) return;

            Connect.Table_Fill("UpdAdr", "select id, type, adr from address where id= " + CreateorUpdatePac.getCurrentAdrRowNumber());
            Adr.Text = Connect.ds.Tables["UpdAdr"].Rows[0]["adr"].ToString();

            for (int i = 0; i < Type.Items.Count; i++)
            {
                if (Type.Items[i].ToString() == Connect.ds.Tables["UpdAdr"].DefaultView[0]["type"].ToString())
                {
                    continue;
                }
                Type.SelectedIndex = i;
            }
        }

        private void Adr_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Adr.Text == "Адрес")
                Adr.Text = "";
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string sql;
            string result;

            MessageBoxButton buttons = MessageBoxButton.YesNo;
            result = MessageBox.Show("Применить изменения?", "Изменения", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                if (CreateorUpdatePac.getCurrentAdrRowNumber() != -1)
                {
                    sql = "update address set type='" + Type.Text + "', adr='" +
                        Adr.Text + "' where id=" + CreateorUpdatePac.getCurrentAdrRowNumber();
                    if (!Connect.Modification_Execute(sql)) return;
                }
                else
                {
                    sql = "insert into address(patient_id, type, adr) values(" + Pacients.getCurrentRowNumber() +
                        ", '" + Type.Text + "', '" + Adr.Text + "')";
                    if (!Connect.Modification_Execute(sql)) return;
                    
                }
            }
            this.Close();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            CreateorUpdatePac.id_adr = -1;
            this.Close();
        }
    }
}
