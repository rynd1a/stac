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
            Connect.Table_Fill("Pac", "select id, (fam || ' ' || name || ' ' || patr) as name from patient where id=" + id_pac);
            Pac.Text = Connect.ds.Tables["Pac"].Rows[0]["name"].ToString();

            return;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            StacSluch.id_sluch = -1;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Connect.Table_Fill("Medic", "select id, (fam || ' ' || name || ' ' || patr) as name from medic");
            Vrach.ItemsSource = Connect.ds.Tables["Medic"].DefaultView;

            if (StacSluch.getCurrentRowNumber() == -1) return;
            ButtonSearch.Visibility = Visibility.Hidden;
            Connect.Table_Fill("UpdSluch", "select s.id, patient_id, (p.fam || ' ' || p.name || ' ' || p.patr) as Пациент, " +
                "medic_id, place_id, diagnosis, s.status, date_create, date_close, result from stac_sluch s " +
                "join patient p on s.patient_id=p.id where s.id=" + StacSluch.getCurrentRowNumber() + " order by s.id");

            Pac.Text = Connect.ds.Tables["UpdSluch"].Rows[0]["Пациент"].ToString();
            Diag.Text = Connect.ds.Tables["UpdSluch"].Rows[0]["diagnosis"].ToString();
            Rez.Text = Connect.ds.Tables["UpdSluch"].Rows[0]["result"].ToString();
            Status.Text = Connect.ds.Tables["UpdSluch"].Rows[0]["status"].ToString();
            OpenS.Text = Convert.ToDateTime(Connect.ds.Tables["UpdSluch"].Rows[0]["date_create"]).ToString();
            if (Connect.ds.Tables["UpdSluch"].Rows[0]["date_close"].ToString() == "") CloseS.Text = "";
            else CloseS.Text = Convert.ToDateTime(Connect.ds.Tables["UpdSluch"].Rows[0]["date_close"]).ToString();

            if (CloseS.Text != "")
            {
                ButtonClose.Visibility = Visibility.Hidden;
                ButtonSearch.Visibility = Visibility.Hidden;
                ButtonSave.Visibility = Visibility.Hidden;

                Diag.IsReadOnly = true;
                Rez.IsReadOnly = true;
                Status.IsReadOnly = true;
                Vrach.IsReadOnly = true;
                Place.IsReadOnly = true;
            }

            for (int i = 0; i < Connect.ds.Tables["Medic"].DefaultView.Count; i++)
            {
                if (
                    Connect.ds.Tables["Medic"].DefaultView[i]["id"].ToString()
                    != Connect.ds.Tables["UpdSluch"].Rows[0]["medic_id"].ToString()
                    ) continue;
               
                Vrach.SelectedIndex = i;
            }
        }

        private void Rez_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Rez.Text == "Результат")
                Rez.Text = "";
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
    }
}
