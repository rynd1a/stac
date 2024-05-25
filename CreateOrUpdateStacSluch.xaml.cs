using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

        private static int id_pac = -1;

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchPac searchPac = new SearchPac();
            searchPac.ShowDialog();

            id_pac = SearchPac.id_pac;
            if (id_pac == -1) return;
            Connect.Table_Fill("Pac", "select id, (fam || ' ' || name || ' ' || patr) as name from patient where id=" + id_pac);
            Pac.Text = Connect.ds.Tables["Pac"].Rows[0]["name"].ToString();

            return;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Fond.action = 0;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Connect.Table_Fill("Medic", "select id, (fam || ' ' || name || ' ' || patr) as name from medic");
            Vrach.ItemsSource = Connect.ds.Tables["Medic"].DefaultView;

            if (Fond.action == 2) return;
            ButtonSearch.Visibility = Visibility.Hidden;
            ButtonSave.Visibility = Visibility.Hidden;
            Rez.Visibility = Visibility.Visible;
            Connect.Table_Fill("UpdSluch", "select s.id, patient_id, (p.fam || ' ' || p.name || ' ' || p.patr) as Пациент, " +
               "medic_id, place_id, diagnosis, s.status, date_create, date_close, result from stac_sluch s " +
               "join patient p on s.patient_id=p.id where s.id=" + Fond.getCurrentRowNumber() + " order by s.id");

            Pac.Text = Connect.ds.Tables["UpdSluch"].Rows[0]["Пациент"].ToString();
            Diag.Text = Connect.ds.Tables["UpdSluch"].Rows[0]["diagnosis"].ToString();
            Status.Text = Connect.ds.Tables["UpdSluch"].Rows[0]["status"].ToString();
            OpenS.Text = Convert.ToDateTime(Connect.ds.Tables["UpdSluch"].Rows[0]["date_create"]).ToString();
            if (Connect.ds.Tables["UpdSluch"].Rows[0]["date_close"].ToString() == "") CloseS.Text = "";
            else CloseS.Text = Convert.ToDateTime(Connect.ds.Tables["UpdSluch"].Rows[0]["date_close"]).ToString();

            for (int i = 0; i < Connect.ds.Tables["Medic"].DefaultView.Count; i++)
            {
                if (
                    Connect.ds.Tables["Medic"].DefaultView[i]["id"].ToString()
                    != Connect.ds.Tables["UpdSluch"].Rows[0]["medic_id"].ToString()
                    ) continue;
               
                Vrach.SelectedIndex = i;
            }

        }

        private void Diag_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Diag.Text == "Диагноз")
                Diag.Text = "";
        }

        private void Status_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Status.Text == "Статус")
                Status.Text = "";
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            string sql;
            string result;

            MessageBoxButton buttons = MessageBoxButton.YesNo;
            result = MessageBox.Show("Применить изменения?", "Изменения", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                sql = "update stac_sluch set diagnosis='" + Diag.Text + "', status='" +
                    Status.Text + "', date_close='" + CloseS.Text + "', result='" +
                    Rez.Text + "' where id=" + Fond.getCurrentRowNumber();
                if (!Connect.Modification_Execute(sql)) return;

                sql = "update bed_place set status='Свободна' where id = (select bed_place_id from bed_place_hospital_room where id=" + Connect.ds.Tables["UpdSluch"].Rows[0]["place_id"].ToString() +")";
                if (!Connect.Modification_Execute(sql)) return;

            }

            this.Close();
        }
    }
}
