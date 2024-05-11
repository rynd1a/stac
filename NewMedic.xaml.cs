using System;
using System.Windows;

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
                if (Medics.getCurrentRowNumber() != -1)
                {
                    sql = "update medic set fam='" + Fam.Text.Replace(" ", "") + "', name='" +
                        Name.Text.Replace(" ", "") + "', patr='" + Patr.Text.Replace(" ", "") + "', department_id=" +
                        id_dep + " where id=" + Medics.getCurrentRowNumber();
                    if (!Connect.Modification_Execute(sql)) return;
                }
                else
                {
                    sql = "insert into medic(fam, name, patr, department_id) values('" + Fam.Text.Replace(" ", "") +
                        "', '" + Name.Text.Replace(" ", "") + "', '" + Patr.Text.Replace(" ", "") + "', " + id_dep + ")";
                    if (!Connect.Modification_Execute(sql)) return;
                }
            }
           
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Name.Text = "Имя";
            Fam.Text = "Фамилия";
            Patr.Text = "Отчество";

            Medics.id_vrach = -1;
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            Connect.Table_Fill("Dep", "select id, name from department");
            Dep.ItemsSource = Connect.ds.Tables["Depart"].DefaultView;

            if (Medics.getCurrentRowNumber() == -1) return;

            Connect.Table_Fill("UpdMedic", "select * from medic where id=" + Medics.getCurrentRowNumber());

            Name.Text = Connect.ds.Tables["UpdMedic"].Rows[0]["name"].ToString().Replace(" ", "");
            Fam.Text = Connect.ds.Tables["UpdMedic"].Rows[0]["fam"].ToString().Replace(" ", "");
            Patr.Text = Connect.ds.Tables["UpdMedic"].Rows[0]["patr"].ToString().Replace(" ", "");

            for (int i = 0; i < Connect.ds.Tables["Dep"].DefaultView.Count; i++)
            {
                if (
                    Connect.ds.Tables["Dep"].DefaultView[i]["id"].ToString()
                    != Connect.ds.Tables["UpdMedic"].Rows[0]["department_id"].ToString()
                    )
                {
                    continue;
                }

                Dep.SelectedIndex = i;
            }
        }
    }
}
