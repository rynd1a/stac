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
using static System.Net.Mime.MediaTypeNames;

namespace stac
{
    /// <summary>
    /// Логика взаимодействия для NewMedic.xaml
    /// </summary>
    public partial class NewMedic : Window
    {
        public NewMedic()
        {
            InitializeComponent();
        }

        int id = Medics.IdVrach();

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Name_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "Имя")
            Name.Text = "";
        }

        private void Fam_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Fam.Text == "Фамилия")
                Fam.Text = "";
        }

        private void Patr_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Patr.Text == "Отчество")
                Patr.Text = "";
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Connect.Table_Fill("Depart", "select id, name from department");
            Dep.ItemsSource = Connect.ds.Tables["Depart"].DefaultView;

            if (id != -1)
            {
                Connect.Table_Fill("UpdMedic", "select * from medic where id=" + id);
                Name.Text = Connect.ds.Tables["UpdMedic"].Rows[0]["name"].ToString().Replace(" ", "");
                Fam.Text = Connect.ds.Tables["UpdMedic"].Rows[0]["fam"].ToString().Replace(" ", "");
                Patr.Text = Connect.ds.Tables["UpdMedic"].Rows[0]["patr"].ToString().Replace(" ", "");
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string sql;
            string result;
            string id_dep = "";

            for (int i = 0; i < Connect.ds.Tables["Depart"].DefaultView.Count; i++)
                if (Connect.ds.Tables["Depart"].DefaultView[i]["name"].ToString() == Dep.Text)
                    id_dep = Connect.ds.Tables["Depart"].DefaultView[i]["id"].ToString();

                    MessageBoxButton buttons = MessageBoxButton.YesNo;
            result = MessageBox.Show("Применить изменения?", "Изменения", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                if (id != -1)
                {
                    sql = "update medic set fam='" + Fam.Text.Replace(" ", "") + "', name='" +
                        Name.Text.Replace(" ", "") + "', patr='" + Patr.Text.Replace(" ", "") + "', department_id=" +
                        id_dep + " where id=" + id;
                    if (!Connect.Modification_Execute(sql))
                        return;
                    //Connect.ds.Tables["Medic"].Rows[id].ItemArray = new object[] { id, Fam.Text, Name.Text, Patr.Text, id_dep };
                    this.Close();
                }
            }
            
        }
    }
}
