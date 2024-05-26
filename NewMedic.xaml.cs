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
            Medics.id_vrach = -1;
            this.Close();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string sql;
            string result;
            string id_dep = "";

            for (int i = 0; i < Connect.ds.Tables["DepartMed"].DefaultView.Count; i++)
                if (Connect.ds.Tables["DepartMed"].DefaultView[i]["id"].ToString() == Dep.SelectedValue.ToString())
                    id_dep = Connect.ds.Tables["DepartMed"].DefaultView[i]["id"].ToString();

            MessageBoxButton buttons = MessageBoxButton.YesNo;
            result = MessageBox.Show("Применить изменения?", "Изменения", buttons).ToString();
            if (result == "No") return;
            else if (result == "Yes")
            {
                if (Medics.getCurrentRowNumber() != -1)
                {
                    sql = "update medic set fam='" + Fam.Text.Replace(" ", "") + "', name='" +
                        Nam.Text.Replace(" ", "") + "', patr='" + Patr.Text.Replace(" ", "") + "', department_id=" +
                        id_dep + " where id=" + Medics.getCurrentRowNumber();
                    if (!Connect.Modification_Execute(sql)) return;
                }
                else
                {
                    sql = "insert into medic(fam, name, patr, department_id) values('" + Fam.Text.Replace(" ", "") +
                        "', '" + Nam.Text.Replace(" ", "") + "', '" + Patr.Text.Replace(" ", "") + "', " + id_dep + ")";
                    if (!Connect.Modification_Execute(sql)) return;
                }
            }
           
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Nam.Text = "";
            Fam.Text = "";
            Patr.Text = "";

            Medics.id_vrach = -1;
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            Connect.Table_Fill("DepartMed", "select id, name from department");
            Dep.ItemsSource = Connect.ds.Tables["DepartMed"].DefaultView;

            if (Medics.getCurrentRowNumber() == -1) return;

            Connect.Table_Fill("UpdMedic", "select * from medic where id=" + Medics.getCurrentRowNumber());

            Nam.Text = Connect.ds.Tables["UpdMedic"].Rows[0]["name"].ToString().Replace(" ", "");
            Fam.Text = Connect.ds.Tables["UpdMedic"].Rows[0]["fam"].ToString().Replace(" ", "");
            Patr.Text = Connect.ds.Tables["UpdMedic"].Rows[0]["patr"].ToString().Replace(" ", "");

            for (int i = 0; i < Connect.ds.Tables["DepartMed"].DefaultView.Count; i++)
            {
                if (
                    Connect.ds.Tables["DepartMed"].DefaultView[i]["id"].ToString()
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
