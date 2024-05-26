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
    /// Логика взаимодействия для CreateOrUpdateBedPlace.xaml
    /// </summary>
    public partial class CreateOrUpdateBedPlace : Window
    {
        public CreateOrUpdateBedPlace()
        {
            InitializeComponent();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            BedPlaceAndRoom.id_bed = -1;
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            BedPlaceAndRoom.id_bed = -1;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (BedPlaceAndRoom.getCurrentBedRowNumber() == -1) return;

            Connect.Table_Fill("UpdBed", "select * from bed_place where id=" + BedPlaceAndRoom.getCurrentBedRowNumber());

            for (int i = 0; i < Prof.Items.Count; i++)
            {
                if (Prof.Items[i].ToString() == Connect.ds.Tables["UpdBed"].DefaultView[0]["type"].ToString()) continue;
                Prof.SelectedIndex = i;
            }

            for (int i = 0; i < Status.Items.Count; i++)
            {
                if (Status.Items[i].ToString() == Connect.ds.Tables["UpdBed"].DefaultView[0]["status"].ToString()) continue;
                Status.SelectedIndex = i;
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string sql;

            MessageBoxButton buttons = MessageBoxButton.YesNo;
            string result = MessageBox.Show("Применить изменения?", "Изменения", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                if (Prof.Text == "")
                {
                    MessageBox.Show("Профиль койки является обязательным для заполнения", "Внимание");
                    return;
                }

                if (Status.Text == "")
                {
                    MessageBox.Show("Статус койки является обязательным для заполнения", "Внимание");
                    return;
                }

                if (BedPlaceAndRoom.getCurrentBedRowNumber() != -1)
                {
                    sql = "update bed_place set type='" + Prof.Text + "', status='" +
                        Status.Text + "' where id=" + BedPlaceAndRoom.getCurrentBedRowNumber();
                    if (!Connect.Modification_Execute(sql)) return;
                }
                else
                {
                    sql = "insert into bed_place(type, status) values('" + Prof.Text +
                        "', '" + Status.Text + "'); insert into bed_place_hospital_room(bed_place_id, hospital_room_id) " +
                        "values ((select max(id) from bed_place), " + BedPlaceAndRoom.getCurrentPalRowNumber() + ")";
                    if (!Connect.Modification_Execute(sql)) return;
                }
            }

            this.Close();
        }
    }
}
